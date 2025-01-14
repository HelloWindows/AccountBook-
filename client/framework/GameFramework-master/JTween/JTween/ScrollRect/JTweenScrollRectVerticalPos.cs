﻿using DG.Tweening;
using Json;

namespace JTween.ScrollRect {
    public class JTweenScrollRectVerticalPos : JTweenBase {
        private float m_beginVerticalPos = 0;
        private float m_toVerticalPos = 0;
        private UnityEngine.UI.ScrollRect m_scrollRect;

        public JTweenScrollRectVerticalPos() {
            m_tweenType = (int)JTweenScrollRect.VerticalPos;
            m_tweenElement = JTweenElement.ScrollRect;
        }

        public float BeginVerticalPos {
            get {
                return m_beginVerticalPos;
            }
            set {
                m_beginVerticalPos = value;
            }
        }

        public float ToVerticalPos {
            get {
                return m_toVerticalPos;
            }
            set {
                m_toVerticalPos = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_scrollRect = m_target.GetComponent<UnityEngine.UI.ScrollRect>();
            if (null == m_scrollRect) return;
            // end if
            m_beginVerticalPos = m_scrollRect.verticalNormalizedPosition;
        }

        protected override Tween DOPlay() {
            if (null == m_scrollRect) return null;
            // end if
            return m_scrollRect.DOHorizontalNormalizedPos(m_toVerticalPos, m_duration, m_isSnapping);
        }

        public override void Restore() {
            if (null == m_scrollRect) return;
            // end if
            m_scrollRect.verticalNormalizedPosition = m_beginVerticalPos;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginVerticalPos")) m_beginVerticalPos = json.GetFloat("beginVerticalPos");
            // end if
            if (json.Contains("vertical")) m_toVerticalPos = json.GetFloat("vertical");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetFloat("beginVerticalPos", m_beginVerticalPos);
            json.SetFloat("vertical", m_toVerticalPos); 
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_scrollRect) {
                errorInfo = GetType().FullName + " GetComponent<ScrollRect> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
