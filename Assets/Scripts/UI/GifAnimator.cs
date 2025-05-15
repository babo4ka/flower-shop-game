using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GifAnimator:MonoBehaviour
{
    [SerializeField] Sprite[] frames;
    [SerializeField] float frameRate = 10f;

    [SerializeField] string spriteSheetName;

    private Image image;
    private float timer;
    private int currentFrame;

    void Start()
    {
        image = GetComponent<Image>();
        LoadSpriteSheetFrames();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f / frameRate)
        {
            timer = 0;
            currentFrame = (currentFrame + 1) % frames.Length;
            image.sprite = frames[currentFrame];
        }
    }

    void LoadSpriteSheetFrames()
    {
        // ��������� ��� ������� �� Resources (��� ���������� Sprite[] � Inspector)
        Sprite[] allSprites = Resources.LoadAll<Sprite>("Sprites/gifs/" + spriteSheetName);
        Debug.Log("Sprites/gifs/" + spriteSheetName);
        Debug.Log(allSprites.Length);

        // ���� ������� ���������� "name_0", "name_1" � �.�., ����� �������������
        frames = allSprites
            .OrderBy(s => s.name, new NaturalSortComparer()) // ���������� "name_1", "name_2"...
            .ToArray();

        // ���� ��� ����������, ����� ������ ����� allSprites
        if (frames == null || frames.Length == 0)
        {
            Debug.LogError("�� ������� ��������� �����! ������� ���� � ��� �������.");
        }
    }


    public class NaturalSortComparer : System.Collections.Generic.IComparer<string>
    {
        public int Compare(string a, string b)
        {
            // ��������� ����� �� ����� (����� � �����)
            string[] partsA = System.Text.RegularExpressions.Regex.Split(a, "([0-9]+)");
            string[] partsB = System.Text.RegularExpressions.Regex.Split(b, "([0-9]+)");

            for (int i = 0; i < Mathf.Min(partsA.Length, partsB.Length); i++)
            {
                // ���� ����� ������, ���������� ��� ����� (���� ��� �����) ��� ��� ������
                if (partsA[i] != partsB[i])
                {
                    int numA, numB;
                    bool isNumA = int.TryParse(partsA[i], out numA);
                    bool isNumB = int.TryParse(partsB[i], out numB);

                    if (isNumA && isNumB)
                    {
                        return numA.CompareTo(numB); // ���������� �����
                    }
                    else
                    {
                        return partsA[i].CompareTo(partsB[i]); // ���������� ������
                    }
                }
            }

            return partsA.Length.CompareTo(partsB.Length);
        }
    }
    
}
