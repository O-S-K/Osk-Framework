using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Injector;
using System.Collections.Generic;
using System;
using UnityEditor.SceneManagement;

public class FillTest
{
    private class SubClassTest
    {
        [GetFromThis]
        public Animator testAnimator;

        [GetFromThis]
        public List<SphereCollider> spheresToGet;
    }

    private class TestBehaviour : MonoBehaviour
    {
        [GetFromThis]
        public Animator myAnimator;

        [GetFromThis]
        public Animator[] myAnimators;

        [GetInChildren]
        public Animator inAnyCaseItsMyAnimator;

        [GetInChildren]
        public List<Animator> myAndChildsAnimator;

        [GetInParent]
        public List<Animator> myAndParentAnimator;

        public List<Animator> shouldStayNull;

        [GetInChildren(typeof(SphereCollider))]
        public List<Collider> spheresOfMineAndMyChild;

        [GetInParent(typeof(SphereCollider))]
        public List<Collider> spheresOfMineAndMyParent;

        [FindObject]
        public Collider someCollider;

        [FindObject(typeof(SphereCollider))]
        public Collider sphereCollider;

        [FindObjectByTag("MainCamera")]
        public GameObject tagObject;

        [GetFromThis]
        public Collider TheCollider
        {
            get;
            private set;
        }

        [GetInParent(typeof(SphereCollider))]
        public Collider[] TheColliders
        {
            get;
            private set;
        }

        [FindObject(typeof(SphereCollider))]
        public List<Collider> foundSpheres;

        [Inject(true)]
        public SubClassTest subclass;

        [Inject]
        public SubClassTest shouldBeNull;
    }

    [Test]
    public void GeneralFillTest()
    {
        var currentScenes = EditorSceneManager.GetAllScenes();
        var currentScenesNames = Array.ConvertAll(currentScenes, x => x.path);
        var isDirty = Array.Find(currentScenes, x => x.isDirty) != null;

        if (isDirty && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            Assert.Fail("Can't run without changing scene.");
            return;
        }

        var testScene = EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene);

        var gameObject = new GameObject("Test object");
        Undo.RegisterCreatedObjectUndo(gameObject, "Created test GameObject");

        var mainCameraGO = new GameObject("Test camera");
        Undo.RegisterCreatedObjectUndo(mainCameraGO, "Created fake camera");
        mainCameraGO.tag = "MainCamera";

        var child = new GameObject("Child game object");
        Undo.RegisterCreatedObjectUndo(child, "Created child of GameObject");
        child.transform.SetParent(gameObject.transform);

        var animatorOnRoot = gameObject.AddComponent<Animator>();
        var animatorOnChild = child.AddComponent<Animator>();

        const int rootSpheresCount = 10;
        List<SphereCollider> rootSpheres = new List<SphereCollider>();
        for (int index = 0; index < rootSpheresCount; index++)
            rootSpheres.Add(gameObject.AddComponent<SphereCollider>());

        const int rootBoxCount = 3;
        for (int index = 0; index < rootBoxCount; index++)
            gameObject.AddComponent<BoxCollider>();

        const int childSpheresCount = 5;
        List<SphereCollider> childSpheres = new List<SphereCollider>();
        for (int index = 0; index < childSpheresCount; index++)
            childSpheres.Add(child.AddComponent<SphereCollider>());

        const int childBoxCount = 3;
        for (int index = 0; index < childBoxCount; index++)
            child.AddComponent<BoxCollider>();

        Assert.NotNull(animatorOnRoot, "Should never be null");
        Assert.NotNull(animatorOnChild, "Should never be null");

        var rootTestBehaviour = gameObject.AddComponent<TestBehaviour>();
        var childTestBehaviour = child.AddComponent<TestBehaviour>();

        Assert.NotNull(rootTestBehaviour, "What? Should never be null");
        Assert.NotNull(childTestBehaviour, "What? Should never be null");

        Assert.DoesNotThrow(() => Resolver.Resolve(gameObject, rootTestBehaviour), "Should not throw!");
        Assert.DoesNotThrow(() => Resolver.Resolve(child, childTestBehaviour), "Should not throw!");

        Assert.IsNotNull(rootTestBehaviour.myAnimator, "Should not be null!");
        Assert.IsTrue(rootTestBehaviour.myAnimator == animatorOnRoot, "Incorrect animator was injected!");

        Assert.IsNotNull(childTestBehaviour.myAnimator, "Should not be null!");
        Assert.IsTrue(childTestBehaviour.myAnimator == animatorOnChild, "Incorrect animator was injected!");

        Assert.IsNotNull(rootTestBehaviour.myAnimators, "Should not be null!");
        Assert.AreEqual(1, rootTestBehaviour.myAnimators.Length, "Should be only one animator!");
        Assert.IsNotNull(childTestBehaviour.myAnimators, "Should not be null!");
        Assert.AreEqual(1, childTestBehaviour.myAnimators.Length, "Should be only one animator!");

        Assert.IsNotNull(rootTestBehaviour.inAnyCaseItsMyAnimator, "Should not be null!");
        Assert.IsTrue(rootTestBehaviour.inAnyCaseItsMyAnimator == animatorOnRoot, "Incorrect animator was injected!");
        Assert.IsNotNull(childTestBehaviour.inAnyCaseItsMyAnimator, "Should not be null!");
        Assert.IsTrue(childTestBehaviour.inAnyCaseItsMyAnimator == animatorOnChild, "Incorrect animator was injected!");

        Assert.IsNotNull(rootTestBehaviour.myAndChildsAnimator, "Should not be null!");
        Assert.AreEqual(2, rootTestBehaviour.myAndChildsAnimator.Count, "Incorrect animators count were injected");
        Assert.IsNotNull(childTestBehaviour.myAndChildsAnimator, "Should not be null!");
        Assert.AreEqual(1, childTestBehaviour.myAndChildsAnimator.Count, "Incorrect animators count were injected");

        Assert.IsNotNull(rootTestBehaviour.myAndParentAnimator, "Should not be null!");
        Assert.AreEqual(1, rootTestBehaviour.myAndParentAnimator.Count, "Incorrect animators count were injected");
        Assert.IsNotNull(childTestBehaviour.myAndParentAnimator, "Should not be null!");
        Assert.AreEqual(2, childTestBehaviour.myAndParentAnimator.Count, "Incorrect animators count were injected");

        Assert.IsTrue(rootTestBehaviour.myAndParentAnimator.Contains(animatorOnRoot), "It's not the Animator we were asking for");
        Assert.IsTrue(
            childTestBehaviour.myAndParentAnimator.Contains(animatorOnRoot) && 
            childTestBehaviour.myAndParentAnimator.Contains(animatorOnChild), "It's not Animators we were asking for");

        Assert.IsTrue(childTestBehaviour.myAndChildsAnimator.Contains(animatorOnChild), "It's not the Animator we were asking for");
        Assert.IsTrue(
            rootTestBehaviour.myAndChildsAnimator.Contains(animatorOnRoot) &&
            rootTestBehaviour.myAndChildsAnimator.Contains(animatorOnChild), "It's not Animators we were asking for");

        Assert.IsNull(rootTestBehaviour.shouldStayNull, "Should stay null");
        Assert.IsNull(childTestBehaviour.shouldStayNull, "Should stay null");

        Assert.AreEqual(childSpheresCount + rootSpheresCount, rootTestBehaviour.spheresOfMineAndMyChild.Count, "Incorrect count of spheres were Injected");
        Assert.AreEqual(childSpheresCount, childTestBehaviour.spheresOfMineAndMyChild.Count, "Incorrect count of spheres were Injected");

        Assert.AreEqual(rootSpheresCount, rootTestBehaviour.spheresOfMineAndMyParent.Count, "Incorrect count of spheres were Injected");
        Assert.AreEqual(childSpheresCount + rootSpheresCount, childTestBehaviour.spheresOfMineAndMyParent.Count, "Incorrect count of spheres were Injected");

        Assert.IsTrue(rootTestBehaviour.spheresOfMineAndMyChild.TrueForAll(x => x.GetType() == typeof(SphereCollider)), "Incorrect type was Injected");
        Assert.IsTrue(childTestBehaviour.spheresOfMineAndMyChild.TrueForAll(x => x.GetType() == typeof(SphereCollider)), "Incorrect type was Injected");

        Assert.IsTrue(rootTestBehaviour.spheresOfMineAndMyParent.TrueForAll(x => x.GetType() == typeof(SphereCollider)), "Incorrect type was Injected");
        Assert.IsTrue(childTestBehaviour.spheresOfMineAndMyParent.TrueForAll(x => x.GetType() == typeof(SphereCollider)), "Incorrect type was Injected");

        Assert.IsTrue(rootTestBehaviour.spheresOfMineAndMyParent.TrueForAll(x => x.gameObject == gameObject), "Incorrect sphere was Injected");
        Assert.IsTrue(childTestBehaviour.spheresOfMineAndMyChild.TrueForAll(x => x.gameObject == child), "Incorrect sphere was Injected");

        Assert.IsNotNull(rootTestBehaviour.someCollider, "Should be found. Error.");
        Assert.IsNotNull(childTestBehaviour.someCollider, "Should be found. Error.");

        Assert.IsNotNull(rootTestBehaviour.sphereCollider, "Should be found. Error.");
        Assert.IsNotNull(childTestBehaviour.sphereCollider, "Should be found. Error.");

        Assert.IsTrue(rootTestBehaviour.sphereCollider is SphereCollider, "Should be SphereCollider. Error.");
        Assert.IsTrue(childTestBehaviour.sphereCollider is SphereCollider, "Should be SphereCollider. Error.");

        Assert.IsNotNull(rootTestBehaviour.tagObject, "Should be found!");
        Assert.IsNotNull(childTestBehaviour.tagObject, "Should be found!");

        Assert.AreEqual(rootTestBehaviour.tagObject.tag, "MainCamera", "Tag should match!");
        Assert.AreEqual(childTestBehaviour.tagObject.tag, "MainCamera", "Tag should match!");

        Assert.IsNotNull(rootTestBehaviour.TheCollider, "Should be Injected!");
        Assert.IsNotNull(childTestBehaviour.TheCollider, "Should be Injected!");
        Assert.IsNotNull(rootTestBehaviour.TheColliders, "Should be Injected!");
        Assert.IsNotNull(childTestBehaviour.TheColliders, "Should be Injected!");

        Assert.AreEqual(rootSpheresCount, rootTestBehaviour.TheColliders.Length, "Incorrect count of spheres were Injected");
        Assert.AreEqual(rootSpheresCount + childSpheresCount, childTestBehaviour.TheColliders.Length, "Incorrect count of spheres were Injected");

        Assert.IsTrue(Array.TrueForAll(rootTestBehaviour.TheColliders, x => x is SphereCollider), "Every of them should be spheres");
        Assert.IsTrue(Array.TrueForAll(childTestBehaviour.TheColliders, x => x is SphereCollider), "Every of them should be spheres");
        Assert.IsTrue(Array.TrueForAll(rootTestBehaviour.TheColliders, x => x.gameObject == gameObject), "Incorrect sphere was Injected");

        Assert.AreEqual(rootSpheresCount + childSpheresCount, rootTestBehaviour.foundSpheres.Count, "Incorrect spheres count was found.");
        Assert.AreEqual(rootSpheresCount + childSpheresCount, childTestBehaviour.foundSpheres.Count, "Incorrect spheres count was found.");

        Assert.AreEqual(rootTestBehaviour.subclass.testAnimator, animatorOnRoot, "Incorrect animator was assigned");
        Assert.AreEqual(childTestBehaviour.subclass.testAnimator, animatorOnChild, "Incorrect animator was assigned");

        Assert.AreEqual(rootTestBehaviour.subclass.spheresToGet.Count, rootSpheresCount, "Incorrect spheres count was injected");
        Assert.AreEqual(childTestBehaviour.subclass.spheresToGet.Count, childSpheresCount, "Incorrect spheres count was injected");

        Assert.IsTrue(rootTestBehaviour.subclass.spheresToGet.TrueForAll(x => x.gameObject == gameObject), "Incorrect sphere were injected");
        Assert.IsTrue(childTestBehaviour.subclass.spheresToGet.TrueForAll(x => x.gameObject == child), "Incorrect sphere were injected");

        Assert.IsNull(rootTestBehaviour.shouldBeNull, "It should stay null!");
        Assert.IsNull(childTestBehaviour.shouldBeNull, "It should stay null!");
    }
}
