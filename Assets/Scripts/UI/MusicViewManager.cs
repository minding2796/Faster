using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameLogic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class MusicViewManager : SingleMono<MusicViewManager>
    {
        public ScrollRect scrollRect;
        public RectTransform contentPanel;

        public HorizontalLayoutGroup layoutGroup;
        public MusicCard musicCardPrefab;
        public int itemCount;

        public float snapForce = 100;
        public List<MusicCard> loopCards = new();
        public List<MusicCard> musicCards = new();
        public List<MusicCard> staticCards = new();
        private int currentItemIndex;

        private bool isSnapped;
        private float snapSpeed;
        private float itemStep;
        private float itemOffset;
        private bool isMoving;

        public MusicCard SelectedMusicCard => musicCards[currentItemIndex];

        private void Start()
        {
            currentItemIndex = -1;
            var arr = GameStatics.GetAllMusicData();
            for (var i = arr.Length - 1; i >= 0; i--)
            {
                var o = Instantiate(musicCardPrefab, Vector3.zero, Quaternion.identity, contentPanel.transform);
                o.transform.localPosition = Vector3.zero;
                o.musicData = arr[i];
                o.gameObject.SetActive(true);
                o.UpdateMusicData();
                staticCards.Insert(0, o);
            }
            musicCards = new List<MusicCard>(staticCards);
            for (var i = 0; i < musicCards.Count; i++) musicCards[musicCards.Count - i - 1].index = 2 - i;

            itemCount = contentPanel.childCount;
            if (itemCount == 0) return;

            // 프리팹을 기준으로 itemStep 계산
            var firstChild = musicCardPrefab.transform as RectTransform;
            if (firstChild != null) itemStep = firstChild.rect.width * firstChild.localScale.x + layoutGroup.spacing;
            if (firstChild != null) itemOffset = firstChild.rect.width * firstChild.localScale.x / 2;
            // Expand를 고려한 오프셋
            itemOffset += 44;

            // 아이템 복제 (앞뒤로 3개씩 더 추가)
            for (var i = 0; i < 3; i++)
            {
                var clone = Instantiate(musicCards[i % musicCards.Count], contentPanel);
                clone.transform.SetAsFirstSibling();
                clone.index = 3 + i;
                loopCards.Add(clone);
            }
            for (var i = 0; i < 3; i++)
            {
                var clone = Instantiate(musicCards[musicCards.Count - i % musicCards.Count - 1], contentPanel);
                clone.index = 2 - musicCards.Count - i;
                loopCards.Add(clone);
            }

            Canvas.ForceUpdateCanvases();
            layoutGroup.enabled = false;
            layoutGroup.enabled = true;

            // 중앙 세트로 위치 초기화
            var pos = contentPanel.anchoredPosition;
            pos.x = -3f * itemStep - itemOffset;
            contentPanel.anchoredPosition = pos;
        }

        private void Update()
        {
            if (itemCount == 0) return;

            // 무한 루프 체크
            var currentX = contentPanel.anchoredPosition.x + itemOffset;
            var threshold = itemCount * itemStep;
            
            if (currentX > -2.5f * itemStep)
            {
                var pos = contentPanel.anchoredPosition;
                pos.x -= threshold;
                contentPanel.anchoredPosition = pos;
            }
            else if (currentX < -threshold - 2.5f * itemStep)
            {
                var pos = contentPanel.anchoredPosition;
                pos.x += threshold;
                contentPanel.anchoredPosition = pos;
            }

            var rawIndex = Mathf.RoundToInt((-contentPanel.anchoredPosition.x - itemOffset) / itemStep) - itemCount;
            var itemIndex = ((rawIndex - 3) % itemCount + itemCount) % itemCount;
            if (itemIndex != currentItemIndex)
            {
                if (currentItemIndex != -1) musicCards[currentItemIndex].Collapse();
                musicCards[itemIndex].Expand();
                currentItemIndex = itemIndex;
            }

            switch (scrollRect.velocity.magnitude)
            {
                case < 200 when !isSnapped:
                {
                    scrollRect.velocity = Vector2.zero;
                    snapSpeed += snapForce * Time.deltaTime;
                    var vector2 = contentPanel.anchoredPosition;
                    var targetX = -(rawIndex + itemCount) * itemStep - itemOffset;
                    vector2.x = Mathf.MoveTowards(vector2.x, targetX, snapSpeed);
                    contentPanel.anchoredPosition = vector2;
                    if (Math.Abs(vector2.x - targetX) < 0.01f) isSnapped = true;
                    break;
                }
                case > 200:
                    isSnapped = false;
                    snapSpeed = 0;
                    break;
            }
        }

        public void MoveToNext(bool isNext = true)
        {
            if (itemCount == 0 || isMoving) return;
            StartCoroutine(MoveToCoroutine(isNext ? 1 : -1, true));
        }

        public void MoveTo(int index)
        {
            if (itemCount == 0 || isMoving) return;
            StartCoroutine(MoveToCoroutine(index));
        }

        private IEnumerator MoveToCoroutine(int target, bool isOffset = false)
        {
            isMoving = true;
            scrollRect.velocity = Vector2.zero;

            var rawIndex = Mathf.RoundToInt((-contentPanel.anchoredPosition.x - itemOffset) / itemStep) -
                           itemCount;
            var startX = contentPanel.anchoredPosition.x;
            var targetIndex = isOffset ? target + rawIndex : target;
            var targetX = -(targetIndex + itemCount) * itemStep - itemOffset;
            
            var elapsed = 0f;
            const float duration = 0.1f;
            var threshold = itemCount * itemStep;
            var teleported = false;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                var t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
                var pos = contentPanel.anchoredPosition;
                pos.x = Mathf.Lerp(startX, targetX, t);

                // Check if we need to teleport during animation
                if (!teleported && t >= 0.5f)
                {
                    var currentX = pos.x + itemOffset;
                    if (currentX > -2.5f * itemStep)
                    {
                        pos.x -= threshold;
                        startX -= threshold;
                        targetX -= threshold;
                        teleported = true;
                    }
                    else if (currentX < -threshold - 2.5f * itemStep)
                    {
                        pos.x += threshold;
                        startX += threshold;
                        targetX += threshold;
                        teleported = true;
                    }
                }

                contentPanel.anchoredPosition = pos;
                yield return null;
            }

            var finalPos = contentPanel.anchoredPosition;
            finalPos.x = targetX;
            contentPanel.anchoredPosition = finalPos;

            isSnapped = true;
            isMoving = false;
        }

        public void FilterCategory(Category category)
        {
            foreach (var card in staticCards)
            {
                card.Collapse();
                card.gameObject.SetActive(false);
            }
            musicCards = staticCards.Where(c => c.musicData.category.Contains(category)).ToList();
            for (var i = 0; i < musicCards.Count; i++) musicCards[musicCards.Count - i - 1].index = 2 - i;
            foreach (var card in musicCards) card.gameObject.SetActive(true);
            RebuildLoop();
        }
        
        public void FilterWord(string word)
        {
            foreach (var card in staticCards)
            {
                card.Collapse();
                card.gameObject.SetActive(false);
            }
            var split = word.Split(" ");
            musicCards = staticCards.Where(c => split.All(c.musicData.IsContains)).ToList();
            for (var i = 0; i < musicCards.Count; i++) musicCards[musicCards.Count - i - 1].index = 2 - i;
            foreach (var card in musicCards) card.gameObject.SetActive(true);
            RebuildLoop();
        }

        private void RebuildLoop()
        {
            currentItemIndex = -1;
            itemCount = musicCards.Count;
            if (itemCount == 0)
            {
                foreach (var musicCard in loopCards) musicCard.gameObject.SetActive(false);
                return;
            }

            // 아이템 복제
            for (var i = 0; i < 3; i++)
            {
                loopCards[i].musicData = musicCards[i % musicCards.Count].musicData;
                loopCards[i].gameObject.SetActive(true);
                loopCards[i].UpdateDummyData();
            }
            for (var i = 0; i < 3; i++)
            {
                loopCards[i + 3].musicData = musicCards[musicCards.Count - i % musicCards.Count - 1].musicData;
                loopCards[i + 3].gameObject.SetActive(true);
                loopCards[i + 3].index = 2 - musicCards.Count - i;
                loopCards[i + 3].UpdateDummyData();
            }
            
            layoutGroup.enabled = false;
            layoutGroup.enabled = true;

            // 중앙 세트로 위치 초기화
            var pos = contentPanel.anchoredPosition;
            pos.x = -3f * itemStep - itemOffset;
            contentPanel.anchoredPosition = pos;
        }
    }
}