﻿using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Engine.Helpers
{
    public class Generics
    {
        private static Dictionary<Type, int> _sizes = new Dictionary<Type, int>();

        public static int SizeOf(Type type)
        {
            int size;
            if (_sizes.TryGetValue(type, out size))
            {
                return size;
            }

            size = SizeOfType(type);
            _sizes.Add(type, size);
            return size;            
        }

        private static int SizeOfType(Type type)
        {
            var dm = new DynamicMethod("SizeOfType", typeof(int), new Type[] { });
            var il = dm.GetILGenerator();
            il.Emit(OpCodes.Sizeof, type);
            il.Emit(OpCodes.Ret);
            return (int)dm.Invoke(null, null);
        }
    }
}