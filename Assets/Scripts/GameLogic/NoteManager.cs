using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace GameLogic
{
    public class NoteManager : SingleMono<NoteManager>
    {
        public int noteIndex;
        public Note notePrefab;
        private Queue<Note> notePool;

        private void Start()
        {
            notePool = new Queue<Note>();
        }

        private void Update()
        {
            var notes = GameStatics.ChartData.notes;
            if (notes.Count <= noteIndex) return;
            while (notes[noteIndex].startTime <= TimeUtils.GetOffsetAffectedTime(Time.time) + 3240 / GameStatics.NoteSpeed)
            {
                var note = notePool.Count > 0 ? notePool.Dequeue() : Instantiate(notePrefab, Vector3.zero, Quaternion.identity, transform);
                note.noteData = notes[noteIndex];
                note.gameObject.SetActive(true);
                note.transform.position = new Vector3(GameStatics.NoteOffset[notes[noteIndex].line].position.x, -50);
                Lines.Instance.AddNote(notes[noteIndex].line, note);
                noteIndex++;
                if (notes.Count <= noteIndex) return;
            }
        }

        public void ReleaseNote(Note note)
        {
            note.gameObject.SetActive(false);
            notePool.Enqueue(note);
        }

        public bool IsThereNoMoreNotes()
        {
            return noteIndex >= GameStatics.ChartData.notes.Count;
        }
    }
}
