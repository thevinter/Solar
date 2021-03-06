﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoomUtils;
using Constraints = System.Tuple<RoomUtils.OpeningType, RoomUtils.OpeningType>;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class RoomSpawner : MonoBehaviour
{
    private RoomTemplates templates;
    private int xcoord;
    private int ycoord;
    private OpeningType roomType;
    private bool spawned;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>() ;
        xcoord = MapProps.PosToMapCoord(transform.position.x);
        ycoord = MapProps.PosToMapCoord(transform.position.y);
        spawned = templates.map[xcoord, ycoord].HasValue;
        Invoke("Spawn", 0.1f);
    }

    // Update is called once per frame
    void Spawn()
    {
        if (!templates.map[xcoord, ycoord].HasValue)
        {

            Constraints c = GetConstraints();
            OpeningType mask = c.Item1;
            OpeningType forced = c.Item2;
            OpeningType invmask = mask ^ OpeningType.all;
            OpeningType additional = (OpeningType) Random.Range(0, (int) OpeningType.all);
            if (templates.toPopulate < 0) additional = OpeningType.none;
            roomType = (forced & mask | additional & invmask);
            templates.lastRoom = Instantiate(templates.rooms[(int) roomType], transform.position, Quaternion.identity) as GameObject;
            spawned = true;
            templates.toPopulate -= 1;
            templates.map[xcoord, ycoord] = roomType;

        }


    }

    public Constraints GetConstraints() {
        OpeningType forcedMask = OpeningType.none;
        OpeningType forcedOpenings = OpeningType.none;

        for (int i=0; i<4; i++) {
            OpeningType direction = (OpeningType) (1 << i);
            OpeningType? neighbor = GetNeighbor(direction);
            if (neighbor is OpeningType nb) {
                forcedMask |= direction;
                if (nb.HasFlag(direction.Opposite())) {
                    forcedOpenings |= direction;
                }
            }
        }
        return new Constraints(forcedMask, forcedOpenings);
    }

    public OpeningType? GetNeighbor(OpeningType direction) {
        int x = xcoord;
        int y = ycoord;
        switch (direction) {
            case OpeningType.bottom: y -= 1; break;
            case OpeningType.top: y += 1; break;
            case OpeningType.left: x -= 1; break;
            case OpeningType.right: x += 1; break;
            default: break; // TODO: error!
        }
        return templates.map[x, y];
    }
}
