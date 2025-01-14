﻿using Json;
using DG.Tweening;

namespace JTween.AudioSource {
    public class JTweenAudioSourcePitch : JTweenBase {
        private float m_beginPitch = 0;
        private float m_toPitch = 0;
        private UnityEngine.AudioSource m_AudioSource;

        public JTweenAudioSourcePitch() {
            m_tweenType = (int)JTweenAudioSource.Pitch;
            m_tweenElement = JTweenElement.AudioSource;
        }
        public float BeginPitch {
            get {
                return m_beginPitch;
            }
            set {
                m_beginPitch = value;
                if (m_beginPitch < 0) {
                    m_beginPitch = 0;
                } else if (m_beginPitch > 1) {
                    m_beginPitch = 1;
                } // end if
            }
        }

        public float ToPitch {
            get {
                return m_toPitch;
            }
            set {
                m_toPitch = value;
                if (m_toPitch < 0) {
                    m_toPitch = 0;
                } else if (m_toPitch > 1) {
                    m_toPitch = 1;
                } // end if
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_AudioSource = m_target.GetComponent<UnityEngine.AudioSource>();
            if (null == m_AudioSource) return;
            // end if
            m_beginPitch = m_AudioSource.volume;
        }

        protected override Tween DOPlay() {
            if (null == m_AudioSource) return null;
            // end if
            return m_AudioSource.DOPitch(m_toPitch, m_duration);
        }

        public override void Restore() {
            if (null == m_AudioSource) return;
            // end if
            m_AudioSource.volume = m_beginPitch;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginPitch")) BeginPitch = json.GetFloat("beginPitch");
            // end if
            if (json.Contains("pitch")) m_toPitch = json.GetFloat("pitch");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetFloat("beginPitch", m_beginPitch);
            json.SetFloat("pitch", m_toPitch);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_AudioSource) {
                errorInfo = "JTweenAudioSourcePitch GetComponent<AudioSource> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
