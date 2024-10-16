using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace CatFramework_TestDOTS.Assets.Scripts.CatFramework_TestDOTS.JobTest
{
    public class JobTestMono : MonoBehaviour
    {
        JobTestSystem jobTestSystem => World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<JobTestSystem>();
        private void Awake()
        {
            enabled = false;
        }
        Mesh mesh;
        private void Start()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            mesh = new Mesh();
            meshFilter.mesh = mesh;
        }
        private void OnDestroy()
        {
            Destroy(mesh);
        }
        public void ScheduleJob1()
        {
            jobTestSystem.JobTest = new JobTest1(mesh);
        }
        public void ScheduleJob2()
        {
            jobTestSystem.JobTest = new JobTest2();
        }
        class JobTest1 : JobTest
        {
            Mesh mesh;
            public JobTest1(Mesh mesh)
            {
                this.mesh = mesh;
            }

            public override JobHandle ScrJob(JobHandle dependOn)
            {
                return new JobTest1Job() { id = 111111 }.Schedule(dependOn);
            }
        }
        class JobTest2 : JobTest
        {
            public override JobHandle ScrJob(JobHandle dependOn)
            {
                return new JobTest2Job() { id = 222222 }.Schedule(dependOn);
            }
        }
        internal struct JobTest1Job : IJob
        {
            [ReadOnly] public int id;
            public void Execute()
            {
            }
        }
        internal struct JobTest2Job : IJob
        {
            [ReadOnly] public int id;
            public void Execute()
            {
                //UnityEngine.Debug.Log($"id:{id}");
            }
        }
    }
}