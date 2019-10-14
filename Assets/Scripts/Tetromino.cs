using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {

    float fall = 0;
    public float fallSpeed = 1;

    public bool allowRotation = true;
    public bool limitRotation = false;
    public string prefabName;

    public int individualScore = 100;

    private float individualScoreTime;

    void Start() {
        
    }

    void Update() {

        CheckUserInput();

        UpdateIndividualScore();
    }

    void UpdateIndividualScore() {

        if(individualScoreTime < 1) {

            individualScoreTime += Time.deltaTime;

        } else {

            individualScoreTime = 0;

            individualScore = Mathf.Max(individualScore - 10, 0);

        }
    }

    void CheckUserInput () {

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            transform.position += new Vector3(1, 0, 0);

            if(CheckIsValidPosition()) {

                FindObjectOfType<Game>().UpdateGrid(this);

            } else {

                transform.position += new Vector3(-1, 0, 0);
            }

        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            transform.position += new Vector3(-1, 0, 0);

            if(CheckIsValidPosition()) {

                FindObjectOfType<Game>().UpdateGrid(this);

            } else {

                transform.position += new Vector3(1, 0, 0);
            }

        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {

            if(allowRotation) {

                if(limitRotation) {

                    if(transform.rotation.eulerAngles.z >= 90) {

                        transform.Rotate(0, 0,-90);

                    } else {

                        transform.Rotate(0,0,90);
                    }

                } else {

                    transform.Rotate(0, 0, 90);
                }

                if(CheckIsValidPosition()) {

                    FindObjectOfType<Game>().UpdateGrid(this);

                } else {

                    if(limitRotation) {

                        if(transform.rotation.eulerAngles.z >= 90) {

                            transform.Rotate(0,0,-90);
                        } else {

                        transform.Rotate(0, 0, 90);
                    }
                } else {

                    transform.Rotate(0,0,-90);
                }
            }
        }

        } else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - fall >= fallSpeed) {

            transform.position += new Vector3(0, -1, 0);

            if(CheckIsValidPosition()) {

                FindObjectOfType<Game>().UpdateGrid(this);

            } else {

                transform.position += new Vector3(0, 1, 0);

                FindObjectOfType<Game>().DeleteRow();

                if(FindObjectOfType<Game>().CheckIsAboveGrid(this)) {

                    FindObjectOfType<Game>().GameOver();
                }

                FindObjectOfType<Game>().SpawnNextTetromino();

                Game.currentScore += individualScore;

                enabled = false;
            }

            fall = Time.time;
        } 
    }

    bool CheckIsValidPosition() {

        foreach (Transform mino in transform) {

            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);

            if (FindObjectOfType<Game>().CheckIsInsideGrid(pos) == false) {

                return false;
            }

            if(FindObjectOfType<Game>().GetTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent != transform) {

                return false;
            }
        }
        return true;
    }
} 
