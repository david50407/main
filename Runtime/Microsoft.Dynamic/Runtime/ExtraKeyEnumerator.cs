/* ****************************************************************************
 *
 * Copyright (c) Microsoft Corporation. 
 *
 * This source code is subject to terms and conditions of the Apache License, Version 2.0. A 
 * copy of the license can be found in the License.html file at the root of this distribution. If 
 * you cannot locate the  Apache License, Version 2.0, please send an email to 
 * dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
 * by the terms of the Apache License, Version 2.0.
 *
 * You must not remove this notice, or any other, from this software.
 *
 *
 * ***************************************************************************/

using System.Diagnostics;
using Microsoft.Scripting.Utils;

namespace Microsoft.Scripting.Runtime {
    class ExtraKeyEnumerator : CheckedDictionaryEnumerator {
        private CustomStringDictionary _idDict;
        private int _curIndex = -1;

        public ExtraKeyEnumerator(CustomStringDictionary idDict) {
            _idDict = idDict;
        }

        protected override object GetKey() {
            return _idDict.GetExtraKeys()[_curIndex];
        }

        protected override object GetValue() {
            object val;
            bool hasExtraValue = _idDict.TryGetExtraValue(_idDict.GetExtraKeys()[_curIndex], out val);
            Debug.Assert(hasExtraValue && !(val is Uninitialized));
            return val;
        }

        protected override bool DoMoveNext() {
            if (_idDict.GetExtraKeys().Length == 0)
                return false;

            while (_curIndex < (_idDict.GetExtraKeys().Length - 1)) {
                _curIndex++;
                object val;
                if (_idDict.TryGetExtraValue(_idDict.GetExtraKeys()[_curIndex], out val) && val != Uninitialized.Instance) {
                    return true;
                }
            }
            return false;
        }

        protected override void DoReset() {
            _curIndex = -1;
        }
    }
}
