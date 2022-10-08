using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    // Fields
    [Tooltip("Reference to the UIDocument object")]
    [SerializeField] private UIDocument UIDoc;
    [Tooltip("Timer script to use for timer display")]
    [SerializeField] private Timer HUDTimer;
    [Tooltip("Sprite to use for hearts display")]
    [SerializeField] private Sprite heartImage;
    [Tooltip("Timer duration in seconds")]
    [SerializeField] private int timerDuration;

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
        timeLabel.text = "" + timerDuration;

        // Add initial hearts
        AddLife(heartsContainer);
        AddLife(heartsContainer);
        AddLife(heartsContainer);

        // Start timer
        HUDTimer.Duration = timerDuration;
        HUDTimer.StartTimer();
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

    public void UpdateTimer()
    {
        // Round time left to nearest second
        int timeLeftSeconds = Mathf.RoundToInt(HUDTimer.TimeLeft());

        // Update timer to show time left
        timeLabel.text = "" + timeLeftSeconds;
    }
}
