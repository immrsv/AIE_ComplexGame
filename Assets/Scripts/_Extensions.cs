using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class _Extensions {

    public static bool HasAny<T>( this T src, T check)  where T : IConvertible
    {
        if (!typeof(T).IsEnum)
            throw new ArgumentException("Arguments must be enums");

        var left = (int)(IConvertible)src;
        var right = (int)(IConvertible)check;

        return (left & right) != 0;
    }

    public static bool HasAll<T>(this T src, T check) where T : IConvertible {
        if (!typeof(T).IsEnum)
            throw new ArgumentException("Arguments must be enums");

        var left = (int)(IConvertible)src;
        var right = (int)(IConvertible)check;

        return (left & right) == right;
    }
}
