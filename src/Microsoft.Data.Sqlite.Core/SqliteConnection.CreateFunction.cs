﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Data.Sqlite
{
    partial class SqliteConnection
    {
        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<TResult>(string name, Func<TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 0, null, SqliteConfiguration.IfNotNull<object, TResult>(function, (s, r) => function()), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<T1, TResult>(string name, Func<T1, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 1, null, SqliteConfiguration.IfNotNull<object, TResult>(function, (s, r) => function(r.GetFieldValue<T1>(0))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<T1, T2, TResult>(string name, Func<T1, T2, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 2, null, SqliteConfiguration.IfNotNull<object, TResult>(function, (s, r) => function(r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<T1, T2, T3, TResult>(string name, Func<T1, T2, T3, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 3, null, SqliteConfiguration.IfNotNull<object, TResult>(function, (s, r) => function(r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<T1, T2, T3, T4, TResult>(string name, Func<T1, T2, T3, T4, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 4, null, SqliteConfiguration.IfNotNull<object, TResult>(function, (s, r) => function(r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<T1, T2, T3, T4, T5, TResult>(string name, Func<T1, T2, T3, T4, T5, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 5, null, SqliteConfiguration.IfNotNull<object, TResult>(function, (s, r) => function(r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<T1, T2, T3, T4, T5, T6, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 6, null, SqliteConfiguration.IfNotNull<object, TResult>(function, (s, r) => function(r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<T1, T2, T3, T4, T5, T6, T7, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 7, null, SqliteConfiguration.IfNotNull<object, TResult>(function, (s, r) => function(r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5), r.GetFieldValue<T7>(6))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 8, null, SqliteConfiguration.IfNotNull<object, TResult>(function, (s, r) => function(r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5), r.GetFieldValue<T7>(6), r.GetFieldValue<T8>(7))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 9, null, SqliteConfiguration.IfNotNull<object, TResult>(function, (s, r) => function(r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5), r.GetFieldValue<T7>(6), r.GetFieldValue<T8>(7), r.GetFieldValue<T9>(8))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the function.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 10, null, SqliteConfiguration.IfNotNull<object, TResult>(function, (s, r) => function(r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5), r.GetFieldValue<T7>(6), r.GetFieldValue<T8>(7), r.GetFieldValue<T9>(8), r.GetFieldValue<T10>(9))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the function.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the function.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 11, null, SqliteConfiguration.IfNotNull<object, TResult>(function, (s, r) => function(r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5), r.GetFieldValue<T7>(6), r.GetFieldValue<T8>(7), r.GetFieldValue<T9>(8), r.GetFieldValue<T10>(9), r.GetFieldValue<T11>(10))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the function.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the function.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the function.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 12, null, SqliteConfiguration.IfNotNull<object, TResult>(function, (s, r) => function(r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5), r.GetFieldValue<T7>(6), r.GetFieldValue<T8>(7), r.GetFieldValue<T9>(8), r.GetFieldValue<T10>(9), r.GetFieldValue<T11>(10), r.GetFieldValue<T12>(11))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the function.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the function.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the function.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the function.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 13, null, SqliteConfiguration.IfNotNull<object, TResult>(function, (s, r) => function(r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5), r.GetFieldValue<T7>(6), r.GetFieldValue<T8>(7), r.GetFieldValue<T9>(8), r.GetFieldValue<T10>(9), r.GetFieldValue<T11>(10), r.GetFieldValue<T12>(11), r.GetFieldValue<T13>(12))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the function.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the function.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the function.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the function.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the function.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 14, null, SqliteConfiguration.IfNotNull<object, TResult>(function, (s, r) => function(r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5), r.GetFieldValue<T7>(6), r.GetFieldValue<T8>(7), r.GetFieldValue<T9>(8), r.GetFieldValue<T10>(9), r.GetFieldValue<T11>(10), r.GetFieldValue<T12>(11), r.GetFieldValue<T13>(12), r.GetFieldValue<T14>(13))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the function.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the function.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the function.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the function.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the function.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the function.</typeparam>
        /// <typeparam name="T15">The type of the fifteenth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 15, null, SqliteConfiguration.IfNotNull<object, TResult>(function, (s, r) => function(r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5), r.GetFieldValue<T7>(6), r.GetFieldValue<T8>(7), r.GetFieldValue<T9>(8), r.GetFieldValue<T10>(9), r.GetFieldValue<T11>(10), r.GetFieldValue<T12>(11), r.GetFieldValue<T13>(12), r.GetFieldValue<T14>(13), r.GetFieldValue<T15>(14))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the function.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the function.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the function.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the function.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the function.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the function.</typeparam>
        /// <typeparam name="T15">The type of the fifteenth parameter of the function.</typeparam>
        /// <typeparam name="T16">The type of the sixteenth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 16, null, SqliteConfiguration.IfNotNull<object, TResult>(function, (s, r) => function(r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5), r.GetFieldValue<T7>(6), r.GetFieldValue<T8>(7), r.GetFieldValue<T9>(8), r.GetFieldValue<T10>(9), r.GetFieldValue<T11>(10), r.GetFieldValue<T12>(11), r.GetFieldValue<T13>(12), r.GetFieldValue<T14>(13), r.GetFieldValue<T15>(14), r.GetFieldValue<T16>(15))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<TResult>(string name, Func<object[], TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, -1, null, SqliteConfiguration.IfNotNull<object, TResult>(function, (s, r) => function(SqliteConfiguration.GetValues(r))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="state">An object available during each invocation of the function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<TState, TResult>(string name, TState state, Func<TState, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 0, state, SqliteConfiguration.IfNotNull<TState, TResult>(function, (s, r) => function(s)), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="state">An object available during each invocation of the function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<TState, T1, TResult>(string name, TState state, Func<TState, T1, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 1, state, SqliteConfiguration.IfNotNull<TState, TResult>(function, (s, r) => function(s, r.GetFieldValue<T1>(0))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="state">An object available during each invocation of the function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<TState, T1, T2, TResult>(string name, TState state, Func<TState, T1, T2, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 2, state, SqliteConfiguration.IfNotNull<TState, TResult>(function, (s, r) => function(s, r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="state">An object available during each invocation of the function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<TState, T1, T2, T3, TResult>(string name, TState state, Func<TState, T1, T2, T3, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 3, state, SqliteConfiguration.IfNotNull<TState, TResult>(function, (s, r) => function(s, r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="state">An object available during each invocation of the function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<TState, T1, T2, T3, T4, TResult>(string name, TState state, Func<TState, T1, T2, T3, T4, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 4, state, SqliteConfiguration.IfNotNull<TState, TResult>(function, (s, r) => function(s, r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="state">An object available during each invocation of the function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<TState, T1, T2, T3, T4, T5, TResult>(string name, TState state, Func<TState, T1, T2, T3, T4, T5, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 5, state, SqliteConfiguration.IfNotNull<TState, TResult>(function, (s, r) => function(s, r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="state">An object available during each invocation of the function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<TState, T1, T2, T3, T4, T5, T6, TResult>(string name, TState state, Func<TState, T1, T2, T3, T4, T5, T6, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 6, state, SqliteConfiguration.IfNotNull<TState, TResult>(function, (s, r) => function(s, r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="state">An object available during each invocation of the function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<TState, T1, T2, T3, T4, T5, T6, T7, TResult>(string name, TState state, Func<TState, T1, T2, T3, T4, T5, T6, T7, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 7, state, SqliteConfiguration.IfNotNull<TState, TResult>(function, (s, r) => function(s, r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5), r.GetFieldValue<T7>(6))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="state">An object available during each invocation of the function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<TState, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(string name, TState state, Func<TState, T1, T2, T3, T4, T5, T6, T7, T8, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 8, state, SqliteConfiguration.IfNotNull<TState, TResult>(function, (s, r) => function(s, r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5), r.GetFieldValue<T7>(6), r.GetFieldValue<T8>(7))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="state">An object available during each invocation of the function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<TState, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(string name, TState state, Func<TState, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 9, state, SqliteConfiguration.IfNotNull<TState, TResult>(function, (s, r) => function(s, r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5), r.GetFieldValue<T7>(6), r.GetFieldValue<T8>(7), r.GetFieldValue<T9>(8))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the function.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="state">An object available during each invocation of the function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<TState, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(string name, TState state, Func<TState, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 10, state, SqliteConfiguration.IfNotNull<TState, TResult>(function, (s, r) => function(s, r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5), r.GetFieldValue<T7>(6), r.GetFieldValue<T8>(7), r.GetFieldValue<T9>(8), r.GetFieldValue<T10>(9))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the function.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the function.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="state">An object available during each invocation of the function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<TState, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(string name, TState state, Func<TState, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 11, state, SqliteConfiguration.IfNotNull<TState, TResult>(function, (s, r) => function(s, r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5), r.GetFieldValue<T7>(6), r.GetFieldValue<T8>(7), r.GetFieldValue<T9>(8), r.GetFieldValue<T10>(9), r.GetFieldValue<T11>(10))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the function.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the function.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the function.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="state">An object available during each invocation of the function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<TState, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(string name, TState state, Func<TState, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 12, state, SqliteConfiguration.IfNotNull<TState, TResult>(function, (s, r) => function(s, r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5), r.GetFieldValue<T7>(6), r.GetFieldValue<T8>(7), r.GetFieldValue<T9>(8), r.GetFieldValue<T10>(9), r.GetFieldValue<T11>(10), r.GetFieldValue<T12>(11))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the function.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the function.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the function.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the function.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="state">An object available during each invocation of the function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<TState, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(string name, TState state, Func<TState, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 13, state, SqliteConfiguration.IfNotNull<TState, TResult>(function, (s, r) => function(s, r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5), r.GetFieldValue<T7>(6), r.GetFieldValue<T8>(7), r.GetFieldValue<T9>(8), r.GetFieldValue<T10>(9), r.GetFieldValue<T11>(10), r.GetFieldValue<T12>(11), r.GetFieldValue<T13>(12))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the function.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the function.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the function.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the function.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the function.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="state">An object available during each invocation of the function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<TState, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(string name, TState state, Func<TState, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 14, state, SqliteConfiguration.IfNotNull<TState, TResult>(function, (s, r) => function(s, r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5), r.GetFieldValue<T7>(6), r.GetFieldValue<T8>(7), r.GetFieldValue<T9>(8), r.GetFieldValue<T10>(9), r.GetFieldValue<T11>(10), r.GetFieldValue<T12>(11), r.GetFieldValue<T13>(12), r.GetFieldValue<T14>(13))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the function.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the function.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the function.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the function.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the function.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the function.</typeparam>
        /// <typeparam name="T15">The type of the fifteenth parameter of the function.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="state">An object available during each invocation of the function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<TState, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(string name, TState state, Func<TState, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, 15, state, SqliteConfiguration.IfNotNull<TState, TResult>(function, (s, r) => function(s, r.GetFieldValue<T1>(0), r.GetFieldValue<T2>(1), r.GetFieldValue<T3>(2), r.GetFieldValue<T4>(3), r.GetFieldValue<T5>(4), r.GetFieldValue<T6>(5), r.GetFieldValue<T7>(6), r.GetFieldValue<T8>(7), r.GetFieldValue<T9>(8), r.GetFieldValue<T10>(9), r.GetFieldValue<T11>(10), r.GetFieldValue<T12>(11), r.GetFieldValue<T13>(12), r.GetFieldValue<T14>(13), r.GetFieldValue<T15>(14))), isDeterministic);

        /// <summary>
        ///     Creates or redefines a SQL function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="name">The name of the SQL function.</param>
        /// <param name="state">An object available during each invocation of the function.</param>
        /// <param name="function">The function to be invoked.</param>
        /// <param name="isDeterministic">Flag indicating whether the function is deterministic.</param>
        public virtual void CreateFunction<TState, TResult>(string name, TState state, Func<TState, object[], TResult> function, bool isDeterministic = false)
            => CreateFunctionCore(name, -1, state, SqliteConfiguration.IfNotNull<TState, TResult>(function, (s, r) => function(s, SqliteConfiguration.GetValues(r))), isDeterministic);
    }
}
