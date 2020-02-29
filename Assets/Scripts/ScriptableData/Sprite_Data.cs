using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sprite_Organizer", menuName = "Sprite_Organizer", order = 1)]
public class Sprite_Data : ScriptableObject {
    [SerializeField] public List<Sprite> sprites;

    public Sprite Get_Randon_Sprite() {
        if (sprites != null) {
            int randon = Random.Range(0, sprites.Count - 1);
            Sprite s = sprites[randon];
            if (s != null) {
                return s;
            } else {
                return null;
            }
        } else {
            return null;
        }
    }
}