﻿using Json;
using DG.Tweening;
using UnityEngine;

namespace JTween.LayoutElement {
    public class JTweenLayoutElementFlexibleSize : JTweenBase {
        private float m_width = 0;
        private float m_height = 0;
        private float m_beginWidth = 0;
        private float m_beginHeight = 0;
        private UnityEngine.UI.LayoutElement m_LayoutElement;

        public JTweenLayoutElementFlexibleSize() {
            m_tweenType = (int)JTweenLayoutElement.FlexibleSize;
            m_tweenElement = JTweenElement.LayoutElement;
        }

        public float Width {
            get {
                return m_width;
            }
            set {
                m_width = value;
            }
        }

        public float Height {
            get {
                return m_height;
            }
            set {
                m_height = value;
            }
        }

        public float BeginWidth {
            get {
                return m_beginWidth;
            }
            set {
                m_beginWidth = value;
            }
        }

        public float BeginHeight {
            get {
                return m_beginHeight;
            }
            set {
                m_beginHeight = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_LayoutElement = m_target.GetComponent<UnityEngine.UI.LayoutElement>();
            if (null == m_LayoutElement) return;
            // end if
            m_beginWidth = m_LayoutElement.flexibleWidth;
            m_beginHeight = m_LayoutElement.flexibleHeight;
        }

        protected override Tween DOPlay() {
            if (null == m_LayoutElement) return null;
            // end if
            return m_LayoutElement.DOFlexibleSize(new Vector2(m_width, m_height), m_duration, m_isSnapping);
        }

        public override void Restore() {
            if (null == m_LayoutElement) return;
            // end if
            m_LayoutElement.flexibleWidth = m_beginWidth;
            m_LayoutElement.flexibleHeight = m_beginHeight;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("width")) m_width = json.GetFloat("width");
            // end if
            if (json.Contains("height")) m_height = json.GetFloat("height");
            // end if
            if (json.Contains("beginWidth")) BeginWidth = json.GetFloat("beginWidth");
            // end if
            if (json.Contains("beginHeight")) BeginHeight = json.GetFloat("beginHeight");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetFloat("width", m_width);
            json.SetFloat("height", m_height);
            json.SetFloat("beginWidth", BeginWidth);
            json.SetFloat("beginHeight", BeginHeight);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_LayoutElement) {
                errorInfo = GetType().FullName + " GetComponent<LayoutElement> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
