﻿// <auto-generated />
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using ToDoListApi.Model;

#pragma warning disable 219, 612, 618
#nullable enable

namespace Entities
{
    [DbContext(typeof(ToDoListContext))]
    public partial class ToDoListContextModel : RuntimeModel
    {
        static ToDoListContextModel()
        {
            var model = new ToDoListContextModel();
            model.Initialize();
            model.Customize();
            _instance = model;
        }

        private static ToDoListContextModel _instance;
        public static IModel Instance => _instance;

        partial void Initialize();

        partial void Customize();
    }
}