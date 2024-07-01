// Copyright (c) 2024 Yuieii.

namespace ue;

public static class UeUtil
{
    public static T With<T>(T subject, Action<T> action)
    {
        action(subject);
        return subject;
    }
}