using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TouchScreen : MonoBehaviour
{
    Score score;
    ParticleSystem particle;
    int colorStop,count, i = 0, hit;
    byte final, firstColor, secondColor;
    byte mainColor;
    Vector2 offsetMaxFirst, offsetMinFirst;
    Color32 background;



    public static bool isPlaying = true;
    Vector3 offsetCube = new Vector3(0, 0.06f, 0);
    public GameObject cube;
    GameObject cam;
    Touch myTouch;
    int randomNumber;

    Transform lastActiveCube;

    List<GameObject> cubes = new List<GameObject>();
    List<GameObject> images = new List<GameObject>();
    //GameObject firstCube;
    Transform lastCube;
    float hangoverX;
    GameObject image;

    Vector3 rightPosition, leftPosition;
    
    
    void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        image = GameObject.FindGameObjectWithTag("Image");
        mainColor = (byte)Random.Range(70, 200);
        ColorNumber();
        randomNumber = Random.Range(1, 3);
        colorStop = ColorNumber();
        final = (byte)Random.Range(6, 9);

        score = GetComponent<Score>();
        ActiveBlock.cubeNumber = 0;
        rightPosition = GameObject.FindGameObjectWithTag("rightPosition").transform.position;
        leftPosition = GameObject.FindGameObjectWithTag("leftPosition").transform.position;

        Instantiate(cube, rightPosition, cube.transform.rotation);
        leftPosition += offsetCube;
        rightPosition += offsetCube;
        FindLastCubes();


        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            myTouch = Input.GetTouch(0);

            if (myTouch.phase == TouchPhase.Began)
            {
                score.ChangeScore();
                ActiveBlock.cubeNumber = ActiveBlock.cubeNumber == 0 ? ActiveBlock.cubeNumber = 1 : ActiveBlock.cubeNumber = 0;
            }
        }


        //Debug.Log(isPlaying);
        Touch();
    }


    void FindLastCubes()
    {
        cubes.Clear();

        var allCubes = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < allCubes.Length; i++)
        {
            cubes.Add(allCubes[i]);

            if (i == 0)
            {

            }
            else
            {
                cubes[i].GetComponent<ActiveBlock>().enabled = false;
            }
        }

        if (cubes.Count >= 0)
        {
            ColorRandom();
            cubes[cubes.Count - 1].GetComponent<ActiveBlock>().enabled = true;
        }
    }

    void HitCubes() // Точное попадание, при hit == 3, расширение кубика
    {
        hit++;

        if( hit == 3)
        {
            particle.Play();
        }
    }

    int ColorNumber()
    {
        int finalColor = Random.Range(0, 2);

        if (finalColor == 0)
        {
            finalColor = 0;
        }
        else if (finalColor == 1)
        {
            finalColor = 50;
        }
        else if (finalColor == 2)
        {
            finalColor = 100;
        }


        firstColor = (byte)((byte)firstColor + (byte)Random.Range(50, 100));
        secondColor = (byte)((byte)firstColor + (byte)Random.Range(50, 100));

        return finalColor;
    }

    private void CreateImageBack()
    {

        Canvas canvas = GameObject.FindGameObjectWithTag("CanvasMain").GetComponent<Canvas>();

        GameObject imagesPlace = GameObject.FindGameObjectWithTag("Images");

        float canvasHeight = GetComponentInParent<Canvas>().pixelRect.height;

        float canvasHeightDel = 100f / count / 100f;

        image.transform.localScale = new Vector3(image.transform.localScale.x, canvasHeightDel, image.transform.localScale.z);

        image.GetComponent<Image>().color = background;

        images.Add(Instantiate(image, canvas.transform.position, image.transform.rotation, imagesPlace.transform));

        if (count > 1)
        {
            for (int i = 0; i < count; i++)
            {
                images[i].transform.localScale = new Vector3(image.transform.localScale.x, canvasHeightDel, image.transform.localScale.z);
            }
            images[0].GetComponent<RectTransform>().offsetMax = new Vector3(0f, -1 * (canvasHeight / 100f * ((1f - canvasHeightDel) * 100f) / 2f), 0f);
            images[0].GetComponent<RectTransform>().offsetMin = new Vector3(0f, -1 * (canvasHeight / 100f * ((1f - canvasHeightDel) * 100f) / 2f), 0f);

            operationImage(canvasHeightDel, canvasHeight);

        }
    }

    void operationImage(float canvasHeightDel, float canvasHeight) // метод который распределяет все изображения по экрану
    {
        if (images.Count > 20)
        {

            for (int i = 0; i < 5; i++)
            {
                Destroy(images[i]);
            }
            images.RemoveRange(0, 5);
            count = images.Count;

            canvasHeightDel = 100f / count / 100f;


            for (int j = 0; j < count; j++)
            {
                images[j].transform.localScale = new Vector3(image.transform.localScale.x, canvasHeightDel, image.transform.localScale.z);
            }


            images[0].GetComponent<RectTransform>().offsetMax = new Vector3(0f, -1 * (canvasHeight / 100f * ((1f - canvasHeightDel) * 100f) / 2f), 0f);
            images[0].GetComponent<RectTransform>().offsetMin = new Vector3(0f, -1 * (canvasHeight / 100f * ((1f - canvasHeightDel) * 100f) / 2f), 0f);
        }

        float q = canvasHeightDel * canvasHeight;

        for (int j = 1; j < count; j++)
        {

            float y = images[j - 1].GetComponent<RectTransform>().anchoredPosition.y;
            images[j].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y + q);
            
        }


    }

    private void ColorRandom()
    {
        count++;

        if (i > final)
        {
            randomNumber = randomNumber == 1 ? (randomNumber = Random.Range(2,3)) : ( randomNumber == 2 ? randomNumber = 3 : randomNumber = 1);

            colorStop = ColorNumber();

            final = (byte)Random.Range(6, 9);

            mainColor = (byte)Random.Range(70, 200);

            i = 0;

            firstColor = (byte)((byte)colorStop + (byte)Random.Range(0, 200));

            secondColor = (byte)((byte)colorStop + (byte)Random.Range(0, 200));
        }



        if (randomNumber == 1)
        {
            background = new Color32((byte)(mainColor - 5 * i), (byte)(firstColor - 5 * i), (byte)(secondColor - 5 * i), 255);
        }
        else if(randomNumber == 2)
        {
            background = new Color32((byte)(firstColor - 5 * i), (byte)(mainColor - 5 * i), (byte)(secondColor - 5 * i), 255);
        }
        else
        {
            background = new Color32((byte)(secondColor - 5 * i), (byte)(firstColor - 5 * i), (byte)(mainColor - 5 * i), 255);
        }


        if (cubes.Count == 2)
        {
            cubes[0].GetComponent<Renderer>().material.color = background;
        }

        if(cubes.Count >= 0)
        {
            cubes[cubes.Count - 1].GetComponent<Renderer>().material.color = background;
        }

        i++;

        CreateImageBack();

    }

    void Touch()
    {
        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.GetTouch(0);

            if (myTouch.phase == TouchPhase.Began)
            {
                lastCube = cubes[cubes.Count - 2].transform;


                HangoX();


                SplitCubeOnZ(hangoverX);

                if (cubes.Count > 0)
                {
                    if (cubes.Count % 2 == 0)
                    {

                        Instantiate(cube, leftPosition, cube.transform.rotation);
                    }
                    else
                    {
                        Instantiate(cube, rightPosition, cube.transform.rotation);
                    }
                }

                FindLastCubes();
                leftPosition += offsetCube;
                rightPosition += offsetCube;

                cam.transform.position += offsetCube / 1.5f;
                particle.transform.position += offsetCube;

            }
        }
    }


    void HangoX()
    {
        if (cubes.Count % 2 == 0)
        {
            hangoverX = cubes[cubes.Count - 1].transform.position.z - lastCube.position.z;
        }
        else
        {
            hangoverX = cubes[cubes.Count - 1].transform.position.x - lastCube.position.x;
        }

    }

    void SplitCubeOnZ(float hangoverX)
    {
        float oldZPosition = 1;
        float newXSize;
        lastActiveCube = cubes[cubes.Count - 1].transform;
        float fallingBlockSize;
        float newZPosition;
        float fallingBlockPosition;


        if (cubes.Count % 2 == 0)
        {
            newXSize = lastCube.localScale.z - Mathf.Abs(hangoverX); 
            fallingBlockSize = lastActiveCube.localScale.z - newXSize;
            newZPosition = lastCube.position.z + (hangoverX / 2);

        }
        else
        {
            newXSize = lastCube.localScale.x - Mathf.Abs(hangoverX); 
            fallingBlockSize = lastActiveCube.localScale.x - newXSize;
            newZPosition = lastCube.position.x + (hangoverX / 2);

        }

        Debug.Log(newZPosition);

        if(newZPosition < 0.03 && newZPosition > -0.03)
        {
            oldZPosition = newZPosition;
            HitCubes();

            if (cubes.Count % 2 == 0)
            {
                newZPosition = lastCube.position.z;
                newXSize = lastCube.transform.localScale.z;
            }
            else
            {
                newZPosition = lastCube.position.x;
                newXSize = lastCube.transform.localScale.x;
            }
        }
        else
        {
            hit = 0;
        }



        if (cubes.Count % 2 == 0)
            {
            if ( !( lastActiveCube.position.z < lastCube.position.z + lastCube.localScale.z && lastActiveCube.position.z > lastCube.position.z - lastCube.localScale.z) )
            {
                isPlaying = false;

                SceneManager.LoadScene(0);
            }


            lastActiveCube.localScale = new Vector3(lastActiveCube.transform.localScale.x, lastActiveCube.localScale.y, newXSize); //

            lastActiveCube.position = new Vector3(lastActiveCube.position.x, lastActiveCube.position.y, newZPosition);


        }
        else
        {
            if ( !( lastActiveCube.position.x < lastCube.position.x + lastCube.localScale.x && lastActiveCube.position.x > lastCube.position.x - lastCube.localScale.x ))
            {
                isPlaying = false;

                SceneManager.LoadScene(0);



                //Debug.Log("Dowland Scene from X");

            }


            lastActiveCube.localScale = new Vector3(newXSize, lastActiveCube.localScale.y, lastActiveCube.localScale.z);
            
            lastActiveCube.position = new Vector3(newZPosition, lastActiveCube.position.y, lastActiveCube.position.z);


        }


        if (hit == 3)
        {
            lastActiveCube.transform.localScale += new Vector3(0.10f, 0, 0.10f);

            if(lastActiveCube.transform.localScale.x >= 1.15 || lastActiveCube.transform.localScale.z >= 1.15)
            {
                var cameraSize = cam.GetComponent<Camera>();
                cameraSize.orthographicSize = 1.8f;
            }

            
            hit = 0;
        }

        if (lastActiveCube.transform.localScale.x < 1.15 || lastActiveCube.transform.localScale.z <= 1.15)
        {
            var cameraSize = cam.GetComponent<Camera>();
            cameraSize.orthographicSize = 1.55f;
        }



            cube = cubes[cubes.Count - 1];
        rightPosition = new Vector3(cube.transform.position.x, rightPosition.y, rightPosition.z);
        leftPosition = new Vector3(leftPosition.x, leftPosition.y, cube.transform.position.z);


        if (cubes.Count % 2 == 0)
        {
            if (lastCube.position.z < lastActiveCube.position.z)
                fallingBlockPosition = lastCube.position.z + hangoverX + lastActiveCube.localScale.z / 2;
            else
                fallingBlockPosition = lastCube.position.z + hangoverX - lastActiveCube.localScale.z / 2;

        }
        else
        {
            if(lastCube.position.x < lastActiveCube.position.x)
                fallingBlockPosition = lastCube.position.x + hangoverX + lastActiveCube.localScale.x / 2;
            else
                fallingBlockPosition = lastCube.position.x + hangoverX - lastActiveCube.localScale.x / 2;



        }

        Debug.Log(oldZPosition);


        if (oldZPosition > 0.03)
            SpawnDropCube(fallingBlockPosition, fallingBlockSize);
    }

    private void SpawnDropCube(float fallingBlockXPosition, float fallingBlockSize)
    {

        if (cubes.Count % 2 == 0)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.localScale = new Vector3(lastActiveCube.localScale.x, lastActiveCube.localScale.y, fallingBlockSize);
            cube.transform.position = new Vector3(lastActiveCube.position.x, lastActiveCube.position.y, fallingBlockXPosition);
            cube.AddComponent<Rigidbody>();
            Destroy(cube, 1f);
        }
        else
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.localScale = new Vector3(fallingBlockSize, lastActiveCube.localScale.y, lastActiveCube.localScale.z);
            cube.transform.position = new Vector3(fallingBlockXPosition, lastActiveCube.position.y, lastActiveCube.position.z);
            cube.AddComponent<Rigidbody>();
            Destroy(cube, 1f);
        }


    }
}
