using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CocktailManager : MonoBehaviour
{
    public SlotManager slotManager; // ���Կ��� ��Ḧ �������� ���� ��ũ��Ʈ
    public GameObject popupPanel; // ����� ǥ���� �˾� �г�
    public Text popupMessage; // �˾� �޽��� �ؽ�Ʈ
    public Image cocktailImage; // ��� �̹����� ǥ���� UI �̹���

    // Inspector���� ������ Sprite
    public Sprite Ʈ���϶���;
    public Sprite �����ø�;
    public Sprite �ƹ氡����;
    public Sprite ����;
    public Sprite ���Ͻ�Ʈ;
    public Sprite Ȧ������;
    public Sprite ���ս���; // ���� �� ǥ���� �⺻ �̹��� (���� ����)

    private Dictionary<string, (string name, Sprite image)> recipes; // ������ ������

    public void Awake()
    {
        InitializeRecipes();
    }

    private void InitializeRecipes()
    {
        // Sprite�� Inspector���� ���� ����
        recipes = new Dictionary<string, (string name, Sprite image)>
        {
            { "����Ʈ ��Ϲ�_0,��Į�� ����_0", ("Ʈ���϶���", Ʈ���϶���) },
            { "��� ��_0,����޺�_0", ("�����ø�", �����ø�) },
            { "����޺�_0,����Ʈ ��Ϲ�_0,�����_0", ("�ƹ氡����", �ƹ氡����) },
            { "���� �޵�_0,Ƽ���� ���� �� ��_0", ("����", ����) },
            { "����޺�_0,��Į�� ����_0,Ƽ���� ���� �� ��_0", ("���Ͻ�Ʈ", ���Ͻ�Ʈ) },
            { "��� ��_0,���� �޵�_0,Ƽ���� ���� �� ��_0", ("Ȧ�� ����", Ȧ������) }
        };
    }

    public void MakeCocktail()
    {
        string[] ingredients = slotManager.GetSlotIngredients();
        System.Array.Sort(ingredients); 
        string recipeKey = string.Join(",", ingredients); 
        Debug.Log($"���Կ��� ��ȯ�� ���: {string.Join(", ", ingredients)}");
        //Debug.Log($"������ Key: {recipeKey}");
        // ������ Ȯ��
        if (recipes.ContainsKey(recipeKey))
        {
            var recipe = recipes[recipeKey];
            ShowPopup(true, recipe.name, recipe.image);
        }
        else
        {
            // ���� �� �⺻ ���� �̹����� �޽��� ǥ��
            ShowPopup(false, "���տ� �����Ͽ����ϴ�...", ���ս���);
        }
    }

    private void ShowPopup(bool success, string message, Sprite image)
    {
        popupPanel.SetActive(true);

        if (success)
        {
            popupMessage.text = $"{message} ���� ����!";
            if (image != null)
            {
                cocktailImage.gameObject.SetActive(true);
                cocktailImage.sprite = image;
            }
            else
            {
                cocktailImage.gameObject.SetActive(false);
            }
        }
        else
        {
            popupMessage.text = message;
            if (image != null)
            {
                cocktailImage.gameObject.SetActive(true);
                cocktailImage.sprite = image;
            }
            else
            {
                cocktailImage.gameObject.SetActive(false);
            }
        }
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
        popupMessage.text = ""; // �޽��� �ʱ�ȭ
        cocktailImage.sprite = null; // �̹��� �ʱ�ȭ
    }
}