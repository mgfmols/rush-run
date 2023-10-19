using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TextureInfo
{
    public float percentageOnTexture;
    public float movementSpeedOnTexture;
    public GroundType type;

    public TextureInfo(float percentageOnTexture, float movementSpeedOnTexture, GroundType type) : this()
    {
        this.percentageOnTexture = percentageOnTexture;
        this.movementSpeedOnTexture = movementSpeedOnTexture;
        this.type = type;
    }
}

public class TerrainChecker : MonoBehaviour
{
    [SerializeField] Controls controls;
    [SerializeField] FootstepSoundScript script;
    [SerializeField] GameObject activeTerrain;
    [SerializeField] int posX;
    [SerializeField] int posZ;
    [SerializeField] TextureInfo[] textureValues;

    void Update()
    {
        GetTerrainTexture();
    }

    void GetTerrainTexture()
    {
        bool playerIsJumping = controls.CheckIsJumping();
        if (!playerIsJumping)
        {
            bool terrainIsValid = CheckTerrain();
            //Debug.Log(terrainIsValid);
            if (terrainIsValid)
            {
                if (activeTerrain.GetComponent<Terrain>() != null)
                {
                    ConvertPosition(activeTerrain.GetComponent<Terrain>());
                    CheckTexture(activeTerrain.GetComponent<Terrain>());
                }
                else if (activeTerrain.GetComponent<SoundType>() != null)
                {
                    ResetTextureValues();
                    CheckObject(activeTerrain.GetComponent<SoundType>());
                }
            }
        }
    }

    bool CheckTerrain()
    {
        GameObject standOnObject = controls.GetStandingOnObject();
        //Debug.Log(standOnObject);
        Terrain terrain = standOnObject.GetComponent<Terrain>();
        if (terrain != null)
        {
            activeTerrain = terrain.gameObject;
            return true;
        }
        SoundType soundType = standOnObject.GetComponent<SoundType>();
        if (soundType != null)
        {
            activeTerrain = soundType.gameObject;
            return true;
        }
        return false;
    }

    void ConvertPosition(Terrain terrain)
    {
        Vector3 terrainPos = gameObject.transform.position - activeTerrain.transform.position;
        Vector3 worldPos = new Vector3(terrainPos.x / terrain.terrainData.size.x, 0, terrainPos.z / terrain.terrainData.size.z);
        posX = (int)(worldPos.x * terrain.terrainData.alphamapWidth);
        posZ = (int)(worldPos.z * terrain.terrainData.alphamapHeight);
    }

    void ResetTextureValues()
    {
        for (int i = 0; i < textureValues.Length; i++)
        {
            textureValues[i].percentageOnTexture = 0;
        }
    }

    void CheckTexture(Terrain terrain) 
    {
        float[,,] aMap = terrain.terrainData.GetAlphamaps(posX, posZ, 1, 1);
        textureValues[0] = new TextureInfo(aMap[0, 0, 0], 0.5f, GroundType.Grass);
        textureValues[1] = new TextureInfo(aMap[0, 0, 1], 0.7f, GroundType.Farmland);
        textureValues[2] = new TextureInfo(aMap[0, 0, 2], 1f, GroundType.Farmland);
        textureValues[3] = new TextureInfo(aMap[0, 0, 3], 0.4f, GroundType.Farmland);
        textureValues[4] = new TextureInfo(aMap[0, 0, 4], 0.5f, GroundType.Farmland);
        textureValues[5] = new TextureInfo(aMap[0, 0, 5], 1f, GroundType.Path);

        GroundType mostStandingOn = GroundType.Grass;
        for(int i = 0; i < textureValues.Length; i++)
        {
            if (i == 0)
            {
                mostStandingOn = textureValues[0].type;
            }
            else
            {
                if (textureValues[i].percentageOnTexture > textureValues[i - 1].percentageOnTexture)
                {
                    mostStandingOn = textureValues[i].type;
                }
            }
        }
        script.UpdateGroundType(mostStandingOn);
    }

    void CheckObject(SoundType type)
    {
        script.UpdateGroundType(type.Type);
    }

    public float GetTerrainSpeed()
    {
        float terrainSpeed = 0f;
        for (int i = 0; i < textureValues.Length; i++)
        {
            terrainSpeed += textureValues[i].percentageOnTexture * textureValues[i].movementSpeedOnTexture;
        }
        if (activeTerrain != null)
        {
            SoundType type = activeTerrain.GetComponent<SoundType>();
            if (type != null)
            {
                terrainSpeed += type.MovementSpeedMultiplier;
                Debug.Log(type.MovementSpeedMultiplier);
            }
        }
        //Debug.Log(terrainSpeed);
        return terrainSpeed;
    }
}
