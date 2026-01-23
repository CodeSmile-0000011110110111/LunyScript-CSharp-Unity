using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LunyScript.Unity.SmokeTests
{
	public sealed class ObjectTestController : MonoBehaviour
	{
		[SerializeField] private Boolean _ObjectCreatedAndDestroyed;
		[SerializeField] private Boolean _EmptyObjectCreated;
		[SerializeField] private Boolean _CubeObjectCreated;
		[SerializeField] private Boolean _SphereObjectCreated;

		private Boolean _foundToBeDestroyedObject;

		private void Awake()
		{
			new GameObject(nameof(ObjectTestLunyScript)); // this causes the corresponding LunyScript to build & run
			StartCoroutine(AssertAtEndOfFrame());
		}

		private void Start()
		{
			var toBeDestroyed = GameObject.Find(ObjectTestLunyScript.DestroyedObjectName);
			_foundToBeDestroyedObject = toBeDestroyed != null;
		}

		private void AssertObjectsCreated()
		{
			var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
			Debug.Log($"Scene has {rootObjects.Length} root objects");

			foreach (var rootObject in rootObjects)
			{
				var goName = rootObject.name;
				_EmptyObjectCreated = _EmptyObjectCreated || goName == ObjectTestLunyScript.EmptyObjectName;
				_CubeObjectCreated = _CubeObjectCreated || goName == ObjectTestLunyScript.CubeObjectName;
				_SphereObjectCreated = _SphereObjectCreated || goName == ObjectTestLunyScript.SphereObjectName;
			}

			_ObjectCreatedAndDestroyed = _foundToBeDestroyedObject && GameObject.Find(ObjectTestLunyScript.DestroyedObjectName) == null;
		}

		private IEnumerator AssertAtEndOfFrame()
		{
			yield return new WaitForEndOfFrame();

			AssertObjectsCreated();
		}
	}

	public sealed class ObjectTestLunyScript : LunyScript
	{
		public const String DestroyedObjectName = "destroyed";
		public const String EmptyObjectName = "empty";
		public const String CubeObjectName = "cube";
		public const String SphereObjectName = "sphere";

		public override void Build()
		{
			When.Self.Created(Object.CreateEmpty(DestroyedObjectName));
			When.Self.Updates(Object.Destroy(DestroyedObjectName));

			When.Self.Ready(Object.CreateEmpty(EmptyObjectName));
			When.Self.Ready(Object.CreateCube(CubeObjectName));
			When.Self.Ready(Object.CreateSphere(SphereObjectName));
		}
	}
}
