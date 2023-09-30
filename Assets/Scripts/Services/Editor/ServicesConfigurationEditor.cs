using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Services.Editor
{
    [CustomEditor(typeof(ServicesConfiguration))]
    public class ServicesConfigurationEditor : UnityEditor.Editor
    {
        private const string IsDisplaySimplifiedNamesKey = "ServicesConfigurationEditor.IsDisplaySimplifiedNames";

        private SerializedProperty _property;
        private ReorderableList _reorderableList;

        private readonly Type _serviceInterfaceType = typeof(IService);
        private readonly Type _unityObjectType = typeof(UnityEngine.Object);

        private bool _isDisplaySimplifiedNames;

        private List<Type> _knownServices;
        private readonly List<Type> _usedServices = new List<Type>(20);

        private void OnEnable()
        {
            _property = serializedObject.FindProperty("Services");

            _reorderableList = new ReorderableList(serializedObject, _property, true, true, true, true);
            _reorderableList.drawHeaderCallback = ListDrawHeaderHandler;
            _reorderableList.elementHeightCallback = ListElementHeightHandler;
            _reorderableList.drawElementCallback = ListDrawElementHandler;
            _reorderableList.onChangedCallback = ListOnChangedHandler;

            _isDisplaySimplifiedNames = EditorPrefs.GetBool(IsDisplaySimplifiedNamesKey, true);

            CollectKnownServices();
            InvalidateList();
        }

        private void ListOnChangedHandler(ReorderableList list)
        {
            InvalidateList();
        }

        public override void OnInspectorGUI()
        {
            DrawCommonControls();

            serializedObject.Update();
        
            EditorGUI.BeginChangeCheck();

            _reorderableList.DoLayoutList();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawCommonControls()
        {
            EditorGUILayout.BeginHorizontal();
            DrawDisplaySimplifiedNamesOption();
            DrawInvalidateButton();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawDisplaySimplifiedNamesOption()
        {
            var isDisplaySimplifiedNames = EditorGUILayout.ToggleLeft("Display simplified names", _isDisplaySimplifiedNames);
            if (isDisplaySimplifiedNames == _isDisplaySimplifiedNames) return;
        
            _isDisplaySimplifiedNames = isDisplaySimplifiedNames;
            EditorPrefs.SetBool(IsDisplaySimplifiedNamesKey, _isDisplaySimplifiedNames);
        }

        private void DrawInvalidateButton()
        {
            if (GUILayout.Button("Invalidate", GUILayout.Width(80)))
            {
                InvalidateList();
            }
        }

        #region Reordable list

        private void ListDrawHeaderHandler(Rect rect)
        {
            EditorGUI.LabelField(rect, "Services with initialization order");
        }

        private float ListElementHeightHandler(int index)
        {
            var defaultHeight = 2 * EditorGUIUtility.singleLineHeight;

            var item = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);

            var validationStateProperty = item.FindPropertyRelative("ValidationState");
            var constructorParametersProperty = item.FindPropertyRelative("ConstructorParameters");
            var implementedInterfacesProperty = item.FindPropertyRelative("ImplementedInterfaces");

            var validationState = (ServiceTypeValidationStateType)validationStateProperty.enumValueIndex;
            switch (validationState)
            {
                case ServiceTypeValidationStateType.KnownType:
                case ServiceTypeValidationStateType.ConstructorRequireUnknownService:
                case ServiceTypeValidationStateType.ConstructorRequireUnityObject:
                case ServiceTypeValidationStateType.ConstructorRequireUnityObjectInstance:
                case ServiceTypeValidationStateType.Valid:
                    defaultHeight += constructorParametersProperty.arraySize * EditorGUIUtility.singleLineHeight;
                    defaultHeight += implementedInterfacesProperty.arraySize * EditorGUIUtility.singleLineHeight;
                    break;
            }

            return defaultHeight;
        }

        private void ListDrawElementHandler(Rect rect, int index, bool isActive, bool isFocused)
        {
            var elementProperty = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            var lineRect = rect;
            lineRect.height = EditorGUIUtility.singleLineHeight;

            var service = (ServiceTypeContainer) elementProperty.boxedValue;
            var hasError = service.ValidationState != ServiceTypeValidationStateType.Valid;

            ListDrawElement_ServiceSelector(ref lineRect, elementProperty, hasError);
            ListDrawElement_StateInfo(ref lineRect, service);

            switch (service.ValidationState)
            {
                case ServiceTypeValidationStateType.UnknownType:
                case ServiceTypeValidationStateType.Duplicated:
                case ServiceTypeValidationStateType.ConstructorCount:
                    break;
                case ServiceTypeValidationStateType.KnownType:
                case ServiceTypeValidationStateType.ConstructorRequireUnknownService:
                case ServiceTypeValidationStateType.ConstructorRequireUnityObject:
                case ServiceTypeValidationStateType.ConstructorRequireUnityObjectInstance:
                case ServiceTypeValidationStateType.Valid:
                    ListDrawElement_NormalState(ref lineRect, elementProperty, service);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (service.ValidationState == ServiceTypeValidationStateType.Valid) return;
        }

        private void ListDrawElement_ServiceSelector(ref Rect rect, SerializedProperty elementProperty, bool hasError)
        {
            var typeNameProperty = elementProperty.FindPropertyRelative("TypeName");

            var assemblyQualified = _knownServices
                .Where(t => !_usedServices.Contains(t) || t.AssemblyQualifiedName == typeNameProperty.stringValue)
                .Select(t => t.AssemblyQualifiedName)
                .ToList();
            if (!assemblyQualified.Contains(typeNameProperty.stringValue)) assemblyQualified.Insert(1, typeNameProperty.stringValue);

            var shortNames = assemblyQualified.Select(SimplifyAssemblyQualifiedName).ToList();

            assemblyQualified.Insert(0, "Undefined");
            shortNames.Insert(0, "Undefined");

            var nameIndex = assemblyQualified.IndexOf(typeNameProperty.stringValue);
            if (nameIndex < 0) nameIndex = 0;

            GUI.color = hasError ? Color.red : Color.green;
            var changedNameIndex = EditorGUI.Popup(rect, nameIndex, _isDisplaySimplifiedNames ? shortNames.ToArray() : assemblyQualified.ToArray());
            GUI.color = Color.white;

            if (changedNameIndex != nameIndex)
            {
                typeNameProperty.stringValue = changedNameIndex > 0 ? assemblyQualified[changedNameIndex] : string.Empty;
                InvalidateList();
            }

            rect.y += EditorGUIUtility.singleLineHeight;
        }

        private void ListDrawElement_StateInfo(ref Rect rect, ServiceTypeContainer service)
        {
            switch (service.ValidationState)
            {
                case ServiceTypeValidationStateType.UnknownType:
                    EditorGUI.LabelField(rect, "Can't find the type");
                    break;
                case ServiceTypeValidationStateType.Duplicated:
                    EditorGUI.LabelField(rect, "Type are duplicated");
                    break;
                case ServiceTypeValidationStateType.ConstructorCount:
                    EditorGUI.LabelField(rect, "Type must contain only one public constructor");
                    break;
                case ServiceTypeValidationStateType.KnownType:
                    EditorGUI.LabelField(rect, "KnownType: this must not happen");
                    break;
                case ServiceTypeValidationStateType.ConstructorRequireUnknownService:
                    EditorGUI.LabelField(rect, "Require unknown yet service");
                    break;
                case ServiceTypeValidationStateType.ConstructorRequireUnityObject:
                    EditorGUI.LabelField(rect, "Construct must accept IService or UnityEngine.Object");
                    break;
                case ServiceTypeValidationStateType.ConstructorRequireUnityObjectInstance:
                    EditorGUI.LabelField(rect, "Object instance require");
                    break;
                case ServiceTypeValidationStateType.Valid:
                    EditorGUI.LabelField(rect, "No issue found");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            rect.y += EditorGUIUtility.singleLineHeight;
        }

        private void ListDrawElement_NormalState(ref Rect rect, SerializedProperty elementProperty, ServiceTypeContainer service)
        {
            ListDrawElement_ConstructorParameters(ref rect, elementProperty, service);
            ListDrawElement_ImplementedInterface(ref rect, elementProperty, service);
        }

        private void ListDrawElement_ConstructorParameters(ref Rect rect, SerializedProperty elementProperty, ServiceTypeContainer service)
        {
            rect.x += EditorGUIUtility.fieldWidth;
            rect.width -= EditorGUIUtility.fieldWidth;

            var constructorParametersProperty = elementProperty.FindPropertyRelative("ConstructorParameters");
            var validationStateParameter = elementProperty.FindPropertyRelative("ValidationState");


            for (var index = 0; index < constructorParametersProperty.arraySize; ++index)
            {
                var parameterProperty = constructorParametersProperty.GetArrayElementAtIndex(index);
                var typeProperty = parameterProperty.FindPropertyRelative("Type");
                var objectProperty = parameterProperty.FindPropertyRelative("Object");
                var typeNameProperty = parameterProperty.FindPropertyRelative("TypeAssemblyQualifiedName");

                var type = (ServiceTypeConstructorParameterType)typeProperty.enumValueIndex;
                switch (type)
                {
                    case ServiceTypeConstructorParameterType.Unknown:
                        EditorGUI.LabelField(rect, $"{index}: Unknown argument type");
                        break;
                    case ServiceTypeConstructorParameterType.Service:
                        EditorGUI.LabelField(rect, $"{index}: {ProcessAssemblyQualifiedName(typeNameProperty.stringValue)}");
                        break;
                    case ServiceTypeConstructorParameterType.Object:
                        var objectType = Type.GetType(typeNameProperty.stringValue);
                        var objectValue = objectProperty.objectReferenceValue;
                        EditorGUI.ObjectField(rect, objectProperty, objectType, new GUIContent($"{index}: {ProcessAssemblyQualifiedName(typeNameProperty.stringValue)}"));
                        if (objectProperty.objectReferenceValue != objectValue) InvalidateList();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                rect.y += EditorGUIUtility.singleLineHeight;
            }

            rect.x -= EditorGUIUtility.fieldWidth;
            rect.width += EditorGUIUtility.fieldWidth;
        }

        private void ListDrawElement_ImplementedInterface(ref Rect rect, SerializedProperty elementProperty, ServiceTypeContainer service)
        {
            rect.x += EditorGUIUtility.fieldWidth;
            rect.width -= EditorGUIUtility.fieldWidth;

            var implementedInterfacesProperty = elementProperty.FindPropertyRelative("ImplementedInterfaces");
            for (var index = 0; index < implementedInterfacesProperty.arraySize; ++index)
            {
                var implementedInterfaceProperty = implementedInterfacesProperty.GetArrayElementAtIndex(index);
                var typeNameProperty = implementedInterfaceProperty.FindPropertyRelative("InterfaceAssemblyQualifiedName");
                var registerProperty = implementedInterfaceProperty.FindPropertyRelative("Register");

                registerProperty.boolValue = EditorGUI.ToggleLeft(rect, ProcessAssemblyQualifiedName(typeNameProperty.stringValue), registerProperty.boolValue);

                rect.y += EditorGUIUtility.singleLineHeight;
            }

            rect.x -= EditorGUIUtility.fieldWidth;
            rect.width += EditorGUIUtility.fieldWidth;
        }

        #endregion

        private void CollectKnownServices()
        {
            var serviceInterfaceType = typeof(IService);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var candidates = assemblies
                .Where(assembly => !assembly.IsDynamic)
                .SelectMany(assembly => assembly.ExportedTypes, (assembly, type) => type)
                .Where(type => type.IsClass && !type.IsAbstract && serviceInterfaceType.IsAssignableFrom(type));

            _knownServices = candidates.ToList();
        }

        private void InvalidateList()
        {
            _property.serializedObject.ApplyModifiedProperties();

            var configurator = (ServicesConfiguration)_property.serializedObject.targetObject;
        
            _usedServices.Clear();
            _usedServices.Capacity = configurator.Services.Count;

            var totalImplementedInterface = new List<Type>(configurator.Services.Count);
            foreach (var service in configurator.Services)
            {
                service.ValidationState = ServiceTypeValidationStateType.UnknownType;

                if (string.IsNullOrEmpty(service.TypeName)) continue;

                //var serviceType = assemblies.Select(assembly => assembly.GetType(service.TypeName)).FirstOrDefault(type => type != null);
                var serviceType = Type.GetType(service.TypeName);
                if (serviceType == null) continue;

                service.ValidationState = ServiceTypeValidationStateType.Duplicated;
                if (_usedServices.Contains(serviceType)) continue;
                _usedServices.Add(serviceType);

                service.ValidationState = ServiceTypeValidationStateType.KnownType;

                var constructors = serviceType.GetConstructors();
                if (constructors.Length != 1)
                {
                    service.ValidationState = ServiceTypeValidationStateType.ConstructorCount;
                    continue;
                }

                service.ValidationState = ServiceTypeValidationStateType.Valid;

                #region Implemented interfaces

                var interfaces = serviceType.GetInterfaces();
                var implementedInterfaces = new List<ImplementedInterfaceData>(interfaces.Length);
                foreach (var implInterface in interfaces)
                {
                    if (implInterface == _serviceInterfaceType) continue;
                    if (!_serviceInterfaceType.IsAssignableFrom(implInterface)) continue;

                    totalImplementedInterface.Add(implInterface);

                    var existsItem = service.ImplementedInterfaces.FirstOrDefault(it => it.InterfaceAssemblyQualifiedName == implInterface.AssemblyQualifiedName);
                    implementedInterfaces.Add(new ImplementedInterfaceData { InterfaceAssemblyQualifiedName = implInterface.AssemblyQualifiedName, Register = existsItem?.Register ?? true});
                }

                service.ImplementedInterfaces = implementedInterfaces;
                #endregion

                #region Constructor
                var constructor = constructors[0];
                var constructorParameters = constructor.GetParameters();
                var constructParametersValues = new List<ServiceTypeConstructorParameterData>(constructorParameters.Length);
                // track only first issue
            
                for (var parameterIndex = 0; parameterIndex < constructorParameters.Length; ++parameterIndex)
                {
                    var parameterInfo = constructorParameters[parameterIndex];
                    if (_serviceInterfaceType.IsAssignableFrom(parameterInfo.ParameterType))
                    {
                        constructParametersValues.Add(new ServiceTypeConstructorParameterData
                        {
                            Type = ServiceTypeConstructorParameterType.Service,
                            TypeAssemblyQualifiedName = parameterInfo.ParameterType.AssemblyQualifiedName
                        });

                        // check if registered
                        if (service.ValidationState != ServiceTypeValidationStateType.Valid) continue;
                        if (_usedServices.Contains(parameterInfo.ParameterType)) continue;
                        if (totalImplementedInterface.Contains(parameterInfo.ParameterType)) continue;

                        service.ValidationState = ServiceTypeValidationStateType.ConstructorRequireUnknownService;

                        continue;
                    }

                    if (_unityObjectType.IsAssignableFrom(parameterInfo.ParameterType))
                    {
                        var parameterObjectValue = parameterIndex < service.ConstructorParameters.Count
                            ? service.ConstructorParameters[parameterIndex]
                            : null;
                        constructParametersValues.Add(new ServiceTypeConstructorParameterData
                        {
                            Type = ServiceTypeConstructorParameterType.Object,
                            Object = parameterObjectValue?.Object,
                            TypeAssemblyQualifiedName = parameterInfo.ParameterType.AssemblyQualifiedName
                        });

                        if (service.ValidationState == ServiceTypeValidationStateType.Valid && parameterObjectValue?.Object == null) service.ValidationState = ServiceTypeValidationStateType.ConstructorRequireUnityObjectInstance;

                        continue;
                    }

                    constructParametersValues.Add(new ServiceTypeConstructorParameterData
                    {
                        Type = ServiceTypeConstructorParameterType.Unknown,
                    });

                    if (service.ValidationState == ServiceTypeValidationStateType.Valid) service.ValidationState = ServiceTypeValidationStateType.ConstructorRequireUnityObject;
                }

                service.ConstructorParameters = constructParametersValues;
                #endregion
            }

            EditorUtility.SetDirty(configurator);
            AssetDatabase.SaveAssetIfDirty(configurator);
            _property.serializedObject.Update();
        }

        private string ProcessAssemblyQualifiedName(string assemblyQualifiedName)
        {
            if (_isDisplaySimplifiedNames) return SimplifyAssemblyQualifiedName(assemblyQualifiedName);

            return assemblyQualifiedName;
        }

        private string SimplifyAssemblyQualifiedName(string assemblyQualifiedName)
        {
            var commaIndex = assemblyQualifiedName.IndexOf(',');
            if (commaIndex == -1) return assemblyQualifiedName;

            return assemblyQualifiedName.Substring(0, commaIndex);
        }
    }
}