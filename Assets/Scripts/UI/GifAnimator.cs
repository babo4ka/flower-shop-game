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
        // Загружаем все спрайты из Resources (или используем Sprite[] в Inspector)
        Sprite[] allSprites = Resources.LoadAll<Sprite>("Sprites/gifs/" + spriteSheetName);
        Debug.Log("Sprites/gifs/" + spriteSheetName);
        Debug.Log(allSprites.Length);

        // Если спрайты называются "name_0", "name_1" и т.д., можно отсортировать
        frames = allSprites
            .OrderBy(s => s.name, new NaturalSortComparer()) // Сортировка "name_1", "name_2"...
            .ToArray();

        // Если нет сортировки, можно просто взять allSprites
        if (frames == null || frames.Length == 0)
        {
            Debug.LogError("Не удалось загрузить кадры! Проверь путь и имя спрайта.");
        }
    }


    public class NaturalSortComparer : System.Collections.Generic.IComparer<string>
    {
        public int Compare(string a, string b)
        {
            // Разделяем имена на части (числа и текст)
            string[] partsA = System.Text.RegularExpressions.Regex.Split(a, "([0-9]+)");
            string[] partsB = System.Text.RegularExpressions.Regex.Split(b, "([0-9]+)");

            for (int i = 0; i < Mathf.Min(partsA.Length, partsB.Length); i++)
            {
                // Если части разные, сравниваем как числа (если это цифры) или как строки
                if (partsA[i] != partsB[i])
                {
                    int numA, numB;
                    bool isNumA = int.TryParse(partsA[i], out numA);
                    bool isNumB = int.TryParse(partsB[i], out numB);

                    if (isNumA && isNumB)
                    {
                        return numA.CompareTo(numB); // Сравниваем числа
                    }
                    else
                    {
                        return partsA[i].CompareTo(partsB[i]); // Сравниваем строки
                    }
                }
            }

            return partsA.Length.CompareTo(partsB.Length);
        }
    }
    
}
