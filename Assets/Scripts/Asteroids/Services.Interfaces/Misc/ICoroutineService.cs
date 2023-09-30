using System.Collections;
using Services;
using UnityEngine;

namespace Asteroids.Services
{
    public interface ICoroutineService : IService
    {
        Coroutine StartCoroutine(IEnumerator coroutineMehod);
        void StopCoroutine(Coroutine coroutine);
    }
}