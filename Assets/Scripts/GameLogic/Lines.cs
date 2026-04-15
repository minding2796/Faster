using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using Utils;

namespace GameLogic
{
    public class Lines : SingleMono<Lines>
    {
        private LinkedList<Note>[] notes;

        public bool IsNoteEmpty()
        {
            return notes.All(x => x.Count == 0);
        }
        
        private void Start()
        {
            notes = new LinkedList<Note>[4];
            for (var i = 0; i < notes.Length; i++) notes[i] = new LinkedList<Note>();
        }

        public void AddNote(int line, Note note)
        {
            notes[line].AddLast(note);
        }

        public void PressLine(int line)
        {
            if (notes[line].Count == 0) return;
            var note = notes[line].First.Value;
            if (note.IsLongNote() && note.noteData.pressed)
            {
                var e = TimeUtils.GetOffsetAffectedTime(Time.time) - (note.noteData.startTime + note.noteData.endOffset);
                if (UserInput.Instance.pressed[note.noteData.line] || GameStatics.AutoPlay)
                {
                    GameStatics.Combo++;
                    GameStatics.MaxCombo = Mathf.Max(GameStatics.Combo, GameStatics.MaxCombo);
                    GameStatics.Score += 1f;
                    PlayManager.Instance.PlayHitEffect(line);
                    UIElementManager.Instance.SetJudge("<color #FFC>Perfect!</color>");
                }
                else
                {
                    GameStatics.Combo = 0;
                    UIElementManager.Instance.SetJudge("<color #C66>Miss..</color>");
                }
                if (!(e >= 0)) return;
                notes[line].RemoveFirst();
                NoteManager.Instance.ReleaseNote(note);
                return;
            }
            var j = Judge(note, line);
            if (j == "UBF") return;
            UIElementManager.Instance.SetJudge(j);
            note.noteData.pressed = true;
            note.noteData.delay = 100;
            
            if (note.IsLongNote()) return;
            notes[line].RemoveFirst();
            NoteManager.Instance.ReleaseNote(note);
        }

        public void ReleaseLine(int line)
        {
            if (GameStatics.AutoPlay) return;
            if (notes[line].Count == 0) return;
            var note = notes[line].First.Value;
            if (!note.IsLongNote() || !note.noteData.pressed) return;
            var e = TimeUtils.GetOffsetAffectedTime(Time.time) - (note.noteData.startTime + note.noteData.endOffset);
            if (e < -110) return;
            GameStatics.Combo++;
            GameStatics.MaxCombo = Mathf.Max(GameStatics.Combo, GameStatics.MaxCombo);
            GameStatics.Score += 1f;
            PlayManager.Instance.PlayHitEffect(line);
            UIElementManager.Instance.SetJudge("<color #FFC>Perfect!</color>");
            notes[line].RemoveFirst();
            NoteManager.Instance.ReleaseNote(note);
        }

        private static string Judge(Note note, int line)
        {
            var e = TimeUtils.GetOffsetAffectedTime(Time.time) - note.noteData.startTime;
            if (e < -150f) return "UBF";
            if (GameStatics.AutoPlay)
            {
                PlayManager.Instance.PlayHitSound();
                PlayManager.Instance.PlayHitEffect(line);
                GameStatics.Score += GameStatics.UnitScore * 1f;
                GameStatics.Combo++;
                GameStatics.MaxCombo = Mathf.Max(GameStatics.Combo, GameStatics.MaxCombo);
                GameStatics.Judges[0]++;
                return "<color #FFC>Perfect!</color>";
            }
            switch (Mathf.Abs(e))
            {
                case <= 50f:
                {
                    PlayManager.Instance.PlayHitSound();
                    PlayManager.Instance.PlayHitEffect(line);
                    GameStatics.Score += GameStatics.UnitScore * 1f;
                    GameStatics.Combo++;
                    GameStatics.MaxCombo = Mathf.Max(GameStatics.Combo, GameStatics.MaxCombo);
                    GameStatics.Judges[0]++;
                    return "<color #FFC>Perfect!</color>";
                }
                case <= 80f:
                {
                    PlayManager.Instance.PlayHitSound();
                    PlayManager.Instance.PlayHitEffect(line);
                    GameStatics.Score += GameStatics.UnitScore * 0.75f;
                    GameStatics.Combo++;
                    GameStatics.MaxCombo = Mathf.Max(GameStatics.Combo, GameStatics.MaxCombo);
                    GameStatics.Judges[1]++;
                    return "<color #DFE>Great!</color>";
                }
                case <= 110f:
                {
                    PlayManager.Instance.PlayHitSound();
                    PlayManager.Instance.PlayHitEffect(line);
                    GameStatics.Score += GameStatics.UnitScore * 0.5f;
                    GameStatics.Combo++;
                    GameStatics.MaxCombo = Mathf.Max(GameStatics.Combo, GameStatics.MaxCombo);
                    GameStatics.Judges[2]++;
                    return "<color #CCF>Good!</color>";
                }
                case <= 150f:
                {
                    GameStatics.Score += GameStatics.UnitScore * 0.1f;
                    GameStatics.Combo++;
                    GameStatics.MaxCombo = Mathf.Max(GameStatics.Combo, GameStatics.MaxCombo);
                    GameStatics.Judges[3]++;
                    return "<color #AAA>Bad..</color>";
                }
                default:
                {
                    GameStatics.Combo = 0;
                    GameStatics.Judges[4]++;
                    return "<color #C66>Miss..</color>";
                }
            }
        }
    }
}
