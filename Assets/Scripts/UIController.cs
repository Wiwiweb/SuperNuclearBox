using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
  public static UIController instance;

  private Label boxScoreLabel;
  private Label bestBoxScoreLabel;

  void Awake() {
      instance = this;
  }

  void Start()
  {
    VisualElement root = GetComponent<UIDocument>().rootVisualElement;
    boxScoreLabel = root.Q<Label>("BoxScore");
    bestBoxScoreLabel = root.Q<Label>("BestBoxScore");
  }

  public void UpdateLabels(int boxScore, int bestBoxScore)
  {
    boxScoreLabel.text = boxScore.ToString();
    bestBoxScoreLabel.text = bestBoxScore.ToString();
  }
}