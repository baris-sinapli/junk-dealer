using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PeriodicFunction.Utils;

public class Cleaning : MonoBehaviour {

    [SerializeField] private Texture2D dirtMaskTextureBase;
    [SerializeField] private Texture2D dirtBrush;
    [SerializeField] private Material material;
    [SerializeField] private TextMeshProUGUI completionRate;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private float rotSpeed = 20;
    // For rotation
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private float distanceToTarget = 100;
    private Vector3 previousPosition;

    private Texture2D dirtMaskTexture;
    private float baseValue;
    private float dirtAmountTotal;
    private float dirtAmount;
    private Vector2Int lastPaintPixelPosition;

    private void Awake() {
        
        dirtMaskTexture = new Texture2D(dirtMaskTextureBase.width, dirtMaskTextureBase.height);
        dirtMaskTexture.SetPixels(dirtMaskTextureBase.GetPixels());
        dirtMaskTexture.Apply();
        material.SetTexture("_DirtMask", dirtMaskTexture);

        dirtAmountTotal = 0f;
        for (int x = 0; x < dirtMaskTextureBase.width; x++) {
            for (int y = 0; y < dirtMaskTextureBase.height; y++) {
                dirtAmountTotal += dirtMaskTextureBase.GetPixel(x, y).g;
            }
        }
        dirtAmount = dirtAmountTotal;

        FunctionPeriodic.Create(() => {
            completionRate.text = 100 - Mathf.RoundToInt(GetDirtAmount() * 100f) + "%";
        }, .03f);
    }

    private void Start()
    {
        baseValue = PlayerPrefs.GetFloat("BaseValue", 0);
        price.text = baseValue.ToString("000");

        target = transform;
    }

    private void Update()
    {
        if(transform.parent.GetComponent<ToggleBrush>().IsBrushActive)
        {
            if (Input.GetMouseButton(0))
            {

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit))
                {
                    Vector2 textureCoord = raycastHit.textureCoord;

                    int pixelX = (int)(textureCoord.x * dirtMaskTexture.width);
                    int pixelY = (int)(textureCoord.y * dirtMaskTexture.height);

                    Vector2Int paintPixelPosition = new Vector2Int(pixelX, pixelY);

                    int paintPixelDistance = Mathf.Abs(paintPixelPosition.x - lastPaintPixelPosition.x) + Mathf.Abs(paintPixelPosition.y - lastPaintPixelPosition.y);
                    int maxPaintDistance = 7;
                    if (paintPixelDistance < maxPaintDistance)
                    {
                        // Painting too close to last position
                        return;
                    }
                    lastPaintPixelPosition = paintPixelPosition;

                    int pixelXOffset = pixelX - (dirtBrush.width / 2);
                    int pixelYOffset = pixelY - (dirtBrush.height / 2);

                    for (int x = 0; x < dirtBrush.width; x++)
                    {
                        for (int y = 0; y < dirtBrush.height; y++)
                        {
                            Color pixelDirt = dirtBrush.GetPixel(x, y);
                            Color pixelDirtMask = dirtMaskTexture.GetPixel(pixelXOffset + x, pixelYOffset + y);

                            float removedAmount = pixelDirtMask.g - (pixelDirtMask.g * pixelDirt.g);
                            dirtAmount -= removedAmount;

                            dirtMaskTexture.SetPixel(
                                pixelXOffset + x,
                                pixelYOffset + y,
                                new Color(0, pixelDirtMask.g * pixelDirt.g, 0)
                            );
                        }
                    }

                    dirtMaskTexture.Apply();
                }
            }

            UpdateSellValue();
        }
        else if(!transform.parent.GetComponent<ToggleBrush>().IsBrushActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 newPosition = cam.ScreenToViewportPoint(Input.mousePosition);
                Vector3 direction = previousPosition - newPosition;

                float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
                float rotationAroundXAxis = direction.y * 180; // camera moves vertically

                cam.transform.position = target.position;

                cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
                cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);

                cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));

                previousPosition = newPosition;
            }
        }
    }

    private void UpdateSellValue()
    {
        float completedPercentage = 100 - Mathf.RoundToInt(GetDirtAmount() * 100f);
        if (completedPercentage <= 15)
            price.text = (baseValue).ToString("000");
        else if (completedPercentage <= 30)
            price.text = (baseValue * 1.2f).ToString("000");
        else if (completedPercentage <= 50)
            price.text = (baseValue * 1.5f).ToString("000");
        else if (completedPercentage <= 75)
            price.text = (baseValue * 2f).ToString("000");
        else if (completedPercentage <= 90)
            price.text = (baseValue * 2.5f).ToString("000");
        else if (completedPercentage <= 99)
            price.text = (baseValue * 3f).ToString("000");
    }

    private float GetDirtAmount() {
        return this.dirtAmount / dirtAmountTotal;
    }

    

    void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;

        transform.Rotate(Vector3.up, -rotX);
        transform.Rotate(Vector3.right, rotY);
    }

}
