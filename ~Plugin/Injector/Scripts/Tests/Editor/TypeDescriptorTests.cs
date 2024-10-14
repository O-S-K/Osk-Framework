using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Injector;
using System.Collections.Generic;

public class TypeDescriptorTests
{
    private class NoInjectorClass
    {
        int val;
        float otherVal;
    }

    private class BasicClass
    {
        [FindObject(typeof(Animator))]
        Animator theAnimator;
    }

    private class PropertyClass
    {
        [GetFromThis]
        public Animator animator
        {
            get;
            private set;
        }
    }

    private class CollectionClass
    {
        [GetInChildren]
        public Animator[] collection;

        [GetInParent]
        public List<Animator> listCollection;
    }

    [IgnoreOnCheck]
    private class TypesNotMatchErrorClass
    {
        [GetInChildren(typeof(Collider))]
        public Animator target;
    }

    [IgnoreOnCheck]
    private class TypesInCollectionNotMatchErrorClass
    {
        [GetInParent(typeof(Collider))]
        Animator[] animators;
    }

    private class TypesMatchClass
    {
        [GetFromThis(typeof(BoxCollider))]
        Collider collider;

        [GetInChildren(typeof(SphereCollider))]
        Collider theSphereCollider;

        [GetInParent(typeof(CapsuleCollider))]
        Collider theCapsuleCollider;

        [GetFromThis(typeof(BoxCollider))]
        List<Collider> colliders;

        [GetInChildren(typeof(CapsuleCollider))]
        Collider[] theCapsules;

        [GetInParent(typeof(SphereCollider))]
        Collider[] spheres;
    }

    [Test]
    public void BasicTest()
    {
        DescriptorsHolder.ClearCache();

        var noInjectorDescriptor = DescriptorsHolder.GetDescriptor(typeof(NoInjectorClass), true);
        Assert.AreEqual(0, noInjectorDescriptor.TargetsCount);

        var propertyDescriptor = DescriptorsHolder.GetDescriptor(typeof(PropertyClass), true);
        Assert.AreEqual(1, propertyDescriptor.TargetsCount, "Incorrect targets count");
        Assert.IsFalse(propertyDescriptor[0].IsCollection, "It should not be collection. Error.");
        Assert.IsTrue(propertyDescriptor[0].ExactMemberType == typeof(Animator), "Type is defined not correctly");

        var collectionClassDescriptor = DescriptorsHolder.GetDescriptor(typeof(CollectionClass), true);
        Assert.AreEqual(2, collectionClassDescriptor.TargetsCount, "Incorrect targets count");
        Assert.IsTrue(collectionClassDescriptor[0].IsCollection, "Should be collection!");
        Assert.IsTrue(collectionClassDescriptor[0].MemberType == typeof(Animator), "Should be Animator!");
        Assert.IsTrue(collectionClassDescriptor[1].IsCollection, "Should be collection!");
        Assert.IsTrue(collectionClassDescriptor[1].MemberType == typeof(Animator), "Should be Animator!");

        var basicClassDescriptor = DescriptorsHolder.GetDescriptor(typeof(BasicClass), true);
        Assert.AreEqual(1, basicClassDescriptor.TargetsCount, "Incorrect types count");
        Assert.IsFalse(basicClassDescriptor[0].IsCollection, "It's not a collection!");
        Assert.IsTrue(basicClassDescriptor[0].MemberType == typeof(Animator));
    }

    [Test]
    public void TypesMissmatchTest()
    {
        DescriptorsHolder.ClearCache();

        Assert.Catch(() => DescriptorsHolder.GetDescriptor(typeof(TypesNotMatchErrorClass), true), "There should be exception!");
        Assert.Catch(() => DescriptorsHolder.GetDescriptor(typeof(TypesInCollectionNotMatchErrorClass), true), "There should be exception!");
    }

    [Test]
    public void TypesMatchTest()
    {
        DescriptorsHolder.ClearCache();

        Assert.DoesNotThrow(() => DescriptorsHolder.GetDescriptor(typeof(TypesMatchClass), true), "Should not throw!");
    }
}
