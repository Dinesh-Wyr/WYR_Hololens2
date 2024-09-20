// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using MixedReality.Toolkit.UX;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    /// <summary>
    /// This class switches back and forth between two symbol boards that otherwise do not fit on the keyboard entirely
    /// </summary>
    public class SymbolKeyboard : MonoBehaviour
    {
        //[Experimental]
        [SerializeField]
        private PressableButton m_PageBck = null;

        [SerializeField]
        private PressableButton m_PageFwd = null;

        private void Update()
        {
            // Visual reflection of state.
            m_PageBck.enabled = NonNativeKeyboard.Instance.IsShifted;
            m_PageFwd.enabled = !NonNativeKeyboard.Instance.IsShifted;
        }
    }
}
