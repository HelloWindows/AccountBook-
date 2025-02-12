﻿using Json;
using DG.Tweening;

namespace JTween.Camera {
    public class JTweenCameraFOV : JTweenBase {
        private float m_beginFOV = 0;
        private float m_toFOV = 0;
        private UnityEngine.Camera m_Camera;

        public JTweenCameraFOV() {
            m_tweenType = (int)JTweenCamera.FOV;
            m_tweenElement = JTweenElement.Camera;
        }

        public float BeginFOV {
            get {
                return m_beginFOV;
            }
            set {
                m_beginFOV = value;
            }
        }

        public float ToFOV {
            get {
                return m_toFOV;
            }
            set {
                m_toFOV = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Camera = m_target.GetComponent<UnityEngine.Camera>();
            if (null == m_Camera) return;
            // end if
            m_beginFOV = m_Camera.fieldOfView;
        }

        protected override Tween DOPlay() {
            if (null == m_Camera) return null;
            // end if
            return m_Camera.DOFieldOfView(m_toFOV, m_duration);
        }

        public override void Restore() {
            if (null == m_Camera) return;
            // end if
            m_Camera.fieldOfView = m_beginFOV;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginFOV")) m_beginFOV = json.GetFloat("beginFOV");
            // end if
            if (json.Contains("FOV")) m_toFOV = json.GetFloat("FOV"); 
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetFloat("beginFOV", m_beginFOV);
            json.SetFloat("FOV", m_toFOV);
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
