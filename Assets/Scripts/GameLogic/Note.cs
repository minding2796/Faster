using System;
using UnityEngine;
using Utils;

namespace GameLogic
{
    public class Note : MonoBehaviour
    {
        public NoteData noteData;
        public SpriteRenderer spriteRenderer;

        private void OnEnable()
        {
            if (GameStatics.AutoPlay) noteData.delay = 0;
        }

        private void FixedUpdate()
        {
            if (!(GetCurrentJudgement() > noteData.delay) && (!noteData.pressed || !(GetEndJudgement() > 0))) return;
            noteData.delay += 100f;
            Lines.Instance.PressLine(noteData.line);
        }

        // 현재 노트 위치로 이동시키기
        private void Update()
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, GetPosition());
            spriteRenderer.size = new Vector2(spriteRenderer.size.x, GetSize());
        }

        private float GetPosition()
        {
            var yPos = (noteData.startTime + noteData.endOffset - TimeUtils.GetOffsetAffectedTime(Time.time)) / 400f * GameStatics.NoteSpeed;
            return GameStatics.NoteOffset[noteData.line].position.y + yPos - (GetSize() - 0.32f) / 2;
        }
        
        private float GetSize()
        {
            return 0.32f + Math.Max(0, noteData.endOffset - Math.Max(0, TimeUtils.GetOffsetAffectedTime(Time.time) - noteData.startTime)) / 400f * GameStatics.NoteSpeed;
        }

        private float GetCurrentJudgement()
        {
            return TimeUtils.GetOffsetAffectedTime(Time.time) - noteData.startTime;
        }

        private float GetEndJudgement()
        {
            return TimeUtils.GetOffsetAffectedTime(Time.time) - (noteData.startTime + noteData.endOffset);
        }
        
        public bool IsLongNote()
        {
            return noteData.endOffset > 0;
        }
    }

    [Serializable]
    public class NoteData
    {
        public float startTime;
        public float endOffset;
        public bool pressed;
        public float delay = 150f;
        public int line;
    }
}

