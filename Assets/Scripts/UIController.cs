using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
  public static UIController instance;

  private Label boxScoreLabel;
  private Label bestBoxScoreLabel;
  private Label deadCenterTextLabel;

  void Awake()
  {
    instance = this;
  }

  void Start()
  {
    VisualElement root = GetComponent<UIDocument>().rootVisualElement;
    boxScoreLabel = root.Q<Label>("BoxScore");
    bestBoxScoreLabel = root.Q<Label>("BestBoxScore");
    deadCenterTextLabel = root.Q<Label>("DeadCenterText");
  }

  public void UpdateScoreLabels(int boxScore, int bestBoxScore)
  {
    boxScoreLabel.text = boxScore.ToString();
    bestBoxScoreLabel.text = bestBoxScore.ToString();
  }

  public void ToggleDeadTextVisible(bool visible)
  {
    if (visible)
    {
      deadCenterTextLabel.style.visibility = Visibility.Visible;
    }
    else
    {
      deadCenterTextLabel.style.visibility = Visibility.Hidden;
    }
  }
}