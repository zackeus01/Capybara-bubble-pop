using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System;

public class SwipController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] int maxPage; 
    int currentPage;
    Vector3 targetPos; 
    [SerializeField] Vector3 pageStep; 
    [SerializeField] RectTransform levelPagesRect; 
    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;
    float dragThreshould;

    private void Awake()
    {
        currentPage = 1; 
        targetPos = levelPagesRect.localPosition;
        dragThreshould = Screen.width / 15;
        GoToPageInstant(1);
    }

    
    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
           MovePage();
        }
    }

    
    public void Previous()
    {
        if (currentPage > 0)
        {
            currentPage--;
           targetPos -= pageStep;
           MovePage();
        }
    }


    public void GoToPageInstant(int pageIndex)
    {
       
        
            targetPos = pageStep/2 * pageIndex;

            LeanTween.moveLocal(levelPagesRect.gameObject, targetPos, tweenTime)
                .setEase(LeanTweenType.easeInOutQuad); 
      
    }


    private void UpdateTargetPos()
    {
        targetPos = pageStep * currentPage;
    }

   
    void MovePage()
    {
        //Vector3 startPos = levelPagesRect.localPosition;
        //float elapsedTime = 0f;

        //while (elapsedTime < tweenTime)
        //{
        //    levelPagesRect.localPosition = Vector3.Lerp(startPos, targetPos, (elapsedTime / tweenTime));
        //    elapsedTime += Time.deltaTime;
        //    yield return null;
        //}

        //levelPagesRect.localPosition = targetPos;

        levelPagesRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);

    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshould)
        {
            if (eventData.position.x > eventData.pressPosition.x) Previous();
            else Next();
        }
        else
        {
           MovePage();
        }
    }
}
