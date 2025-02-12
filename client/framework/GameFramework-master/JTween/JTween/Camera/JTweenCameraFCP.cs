﻿using DG.Tweening;
using Json;

namespace JTween.Camera {
    public class JTweenCameraFCP : JTweenBase {
        private float m_beginFCP = 0;
        private float m_toFCP = 0;
        private UnityEngine.Camera m_Camera;

        public JTweenCameraFCP() {
            m_tweenType = (int)JTweenCamera.FCP;
            m_tweenElement = JTweenElement.Camera;
        }

        public float BeginFCP {
            get {
                return m_beginFCP;
            }
            set {
                m_beginFCP = value;
            }
        }

        public float ToFCP {
            get {
                return m_toFCP;
            }
            set {
                m_toFCP = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Camera = m_target.GetComponent<UnityEngine.Camera>();
            if (null == m_Camera) return;
            // end if
            m_beginFCP = m_Camera.farClipPlane;
        }

        protected override Tween DOPlay() {
            if (null == m_Camera) return null;
            // end if
            return m_Camera.DOFarClipPlane(m_toFCP, m_duration);
        }

        public override void Restore() {
            if (null == m_Camera) return;
            // end if
            m_Camera.farClipPlane = m_beginFCP;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginFCP")) m_beginFCP = json.GetFloat("beginFCP");
            // end if
            if (json.Contains("FCP")) m_toFCP = json.GetFloat("FCP");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetFloat("beginFCP", m_beginFCP);
            json.SetFloat("FCP", m_toFCP);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Camera) {
                errorInfo = GetType().FullName + " GetComponent<Camera> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
