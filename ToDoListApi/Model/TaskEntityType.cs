﻿// <auto-generated />
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ToDoListApi.Entities;
using Task = ToDoListApi.Entities.Task;

#pragma warning disable 219, 612, 618
#nullable enable

namespace Entities
{
    internal partial class TaskEntityType
    {
        public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType? baseEntityType = null)
        {
            var runtimeEntityType = model.AddEntityType(
                "ToDoListApi.Entities.Task",
                typeof(Task),
                baseEntityType);

            var taskId = runtimeEntityType.AddProperty(
                "TaskId",
                typeof(long),
                propertyInfo: typeof(Task).GetProperty("TaskId", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Task).GetField("<TaskId>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueGenerated: ValueGenerated.OnAdd,
                afterSaveBehavior: PropertySaveBehavior.Throw);
            taskId.AddAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            var taskContent = runtimeEntityType.AddProperty(
                "TaskContent",
                typeof(string),
                propertyInfo: typeof(Task).GetProperty("TaskContent", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Task).GetField("<TaskContent>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                maxLength: 256);

            var taskList = runtimeEntityType.AddProperty(
                "TaskList",
                typeof(int),
                propertyInfo: typeof(Task).GetProperty("TaskList", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Task).GetField("<TaskList>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var taskStatus = runtimeEntityType.AddProperty(
                "TaskStatus",
                typeof(bool),
                propertyInfo: typeof(Task).GetProperty("TaskStatus", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Task).GetField("<TaskStatus>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var key = runtimeEntityType.AddKey(
                new[] { taskId });
            runtimeEntityType.SetPrimaryKey(key);

            var index = runtimeEntityType.AddIndex(
                new[] { taskList });

            return runtimeEntityType;
        }

        public static RuntimeForeignKey CreateForeignKey1(RuntimeEntityType declaringEntityType, RuntimeEntityType principalEntityType)
        {
            var runtimeForeignKey = declaringEntityType.AddForeignKey(new[] { declaringEntityType.FindProperty("TaskList")! },
                principalEntityType.FindKey(new[] { principalEntityType.FindProperty("ListId")! })!,
                principalEntityType,
                deleteBehavior: DeleteBehavior.Cascade,
                required: true);

            var taskListNavigation = declaringEntityType.AddNavigation("TaskListNavigation",
                runtimeForeignKey,
                onDependent: true,
                typeof(List),
                propertyInfo: typeof(Task).GetProperty("TaskListNavigation", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Task).GetField("<TaskListNavigation>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var tasks = principalEntityType.AddNavigation("Tasks",
                runtimeForeignKey,
                onDependent: false,
                typeof(ICollection<Task>),
                propertyInfo: typeof(List).GetProperty("Tasks", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(List).GetField("<Tasks>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            runtimeForeignKey.AddAnnotation("Relational:Name", "TaskList");
            return runtimeForeignKey;
        }

        public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
        {
            runtimeEntityType.AddAnnotation("Relational:FunctionName", null);
            runtimeEntityType.AddAnnotation("Relational:Schema", null);
            runtimeEntityType.AddAnnotation("Relational:SqlQuery", null);
            runtimeEntityType.AddAnnotation("Relational:TableName", "Tasks");
            runtimeEntityType.AddAnnotation("Relational:ViewName", null);
            runtimeEntityType.AddAnnotation("Relational:ViewSchema", null);

            Customize(runtimeEntityType);
        }

        static partial void Customize(RuntimeEntityType runtimeEntityType);
    }
}
