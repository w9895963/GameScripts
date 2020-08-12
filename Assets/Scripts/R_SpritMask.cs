﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class R_SpritMask : MonoBehaviour {


    public GameObject drawObjectTemplate;
    private GameObject drawObj;
    void Start () {

        // SpriteRenderer refSpriteRender = GetComponent<SpriteRenderer> ();
        // Fn.LayerRender data = new Fn.LayerRender (refSpriteRender, front.bounds);

        // SpriteRenderer sprite = new GameObject ("Sprite").AddComponent<SpriteRenderer> ();

        // sprite.sprite = data.RenderToSprite (LayerMask.GetMask ("Temp"));

        // sprite.transform.position = data.spritePosition;
        // sprite.transform.localScale = data.scale;



    }

    private void Awake () {
        Fn.AddEventToTrigger (gameObject, EventTriggerType.BeginDrag, BeginDrag);
        Fn.AddEventToTrigger (gameObject, EventTriggerType.Drag, Drag);
        Fn.AddEventToTrigger (gameObject, EventTriggerType.EndDrag, EndDrag);
    }

    private void EndDrag (BaseEventData arg0) {
        if (enabled) {
            PointerEventData data = (PointerEventData) arg0;

            drawObj.GetComponent<SpriteRenderer> ().enabled = false;
            drawObj.GetComponent<SpriteMask> ().enabled = true;
            drawObj.layer = LayerMask.NameToLayer ("Temp");

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer> ();
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;


            Fn.LayerRender render;
            SpriteRenderer spriteMask = drawObj.GetComponent<SpriteRenderer> ();
            render = new Fn.LayerRender (spriteRenderer, spriteMask.bounds);
            spriteMask.sprite = render.RenderToSprite (LayerMask.GetMask ("Temp"));

            Fn.LayerRender render2;
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            render2 = new Fn.LayerRender (spriteRenderer, spriteRenderer.bounds);
            spriteRenderer.sprite = render2.RenderToSprite (LayerMask.GetMask ("Temp"));
            Destroy (spriteRenderer.GetComponent<PolygonCollider2D> ());
            spriteRenderer.gameObject.AddComponent<PolygonCollider2D> ().isTrigger = true;


            spriteMask.color = Color.white;
            spriteMask.enabled = true;
            drawObj.transform.position = render.spritePosition;
            drawObj.transform.localScale = render.scale;
            drawObj.layer = 0;
            Destroy (spriteMask.GetComponent<SpriteMask> ());
            drawObj.AddComponent<Rigidbody2D> ();
            drawObj.AddComponent<PolygonCollider2D> ();
            drawObj.AddComponent<ConstantForce2D> ().force = Vector2.down * 20;




        }
    }

    private void Drag (BaseEventData arg0) {
        if (enabled) {
            PointerEventData data = (PointerEventData) arg0;

            Vector2 position = drawObj.transform.position;
            Vector2 pointer = Camera.main.ScreenToWorldPoint (data.position);
            float scale = (pointer - position).magnitude;
            scale = Mathf.Clamp (scale, 0, 4);
            drawObj.transform.localScale = new Vector3 (scale, scale, 1);
        }
    }

    private void BeginDrag (BaseEventData arg0) {
        if (enabled) {
            PointerEventData data = (PointerEventData) arg0;

            drawObj = GameObject.Instantiate (drawObjectTemplate);
            Vector3 p = Camera.main.ScreenToWorldPoint (data.pressPosition);
            p.z = 0;
            drawObj.transform.position = p;
        }
    }
}