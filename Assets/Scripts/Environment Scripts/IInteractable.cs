using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    /// <summary>
    /// function for handling interaction
    /// </summary>
    /// <param name="interactorTransform"></param>
    void Interact();

    /// <summary>
    /// Function that sets the interaction UI text
    /// </summary>
    /// <returns>string that shows up for the interaction UI</returns>

    Transform GetTransform();
}
