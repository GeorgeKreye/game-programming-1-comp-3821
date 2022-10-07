using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    [Tooltip("Reference to the UIDocument object")]
    [SerializeField] private UIDocument UIDoc;

    [Tooltip("Starting time s")]
    [SerializeField] private float levelTime;
    [SerializeField] private Sprite heartImage;

    // Elements to modify
    private VisualElement heartsContainer;
    private Label timeLabel;

    // Start is called before the first frame update
    void Start()
    {
        // Get elements
        VisualElement root = UIDoc.rootVisualElement;
        timeLabel = root.Q<Label>("TimerValue");
        heartsContainer = root.Q<VisualElement>("Lives");

        // Set initial timer
        timeLabel.text = "" + levelTime;

        // Add initial hearts
        AddLife(heartsContainer);
        AddLife(heartsContainer);
        AddLife(heartsContainer);
    }

    // Update is called once per frame
    void Update()
    {
        // Unused
    }

    public void AddLife(VisualElement container)
    {
        // Set heart image
        Image heart = new Image();
        heart.sprite = heartImage;

        // Set padding and size
        heart.style.paddingTop = 5;
        heart.style.paddingLeft = 0;
        heart.style.paddingRight = 0;
        heart.style.width = 32;
        heart.style.height = 32;

        // Add to container
        container.Add(heart);
    }
}
