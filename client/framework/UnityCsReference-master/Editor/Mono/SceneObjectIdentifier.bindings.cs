// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using UnityEngine.Bindings;

namespace UnityEditor
{
    [NativeHeader("Editor/Src/SceneObjectIdentifier.h")]
    struct SceneObjectIdentifier
    {
        internal ulong TargetObject;
        internal ulong TargetPrefab;
    }
}
