using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Events;

public class CreditsGameManager : MonoBehaviourSingleton<CreditsGameManager>
{
    [SerializeField]
    private List<Contributors> creditsList;
    [SerializeField] GameObject creditPrefab;
    
    [SerializeField] public Transform[] locations;
    public float headerWaitTime, contributorWaitTime, finalWaitTime;
    // Add more methods and functionality as needed

     public UnityEvent OnCreditEnds;

    private void Start()
    {
        StartCoroutine(DisplayCredits(creditsList));
    }

    private IEnumerator DisplayCredits(List<Contributors> creditsList)
    {
        foreach (var roleContributors in creditsList)
        {
            GameObject headerCredits = Instantiate(creditPrefab, locations[0]);
            headerCredits.GetComponent<DisplayCredits>().Initialize("<b>" + roleContributors.contributorRole + ":</b>", 2.0f);

            yield return new WaitForSeconds(headerWaitTime);

            int i = 1;
            foreach (var contributor in roleContributors.contributors)
            {
                GameObject contributorName = Instantiate(creditPrefab, locations[i]);
                contributorName.GetComponent<DisplayCredits>().Initialize("- " + contributor, 1.0f);

                i=(i+1)%locations.Length;
                yield return new WaitForSeconds(contributorWaitTime);
            }
        }
        Instantiate(creditPrefab, locations[1]).GetComponent<DisplayCredits>().Initialize("<b> THANKS FOR PLAYING <b>", 1.0f);
        yield return new WaitForSeconds(finalWaitTime);
        OnCreditEnds.Invoke();
    }

    private Transform[] GiveRandomSpawnPos()
    {
        Transform localization1 = locations[Random.Range(0, locations.Length)];
        Transform localization2 = locations[Random.Range(0, locations.Length)];
        if (localization1 == localization2) 
        {
            return new[] { localization1, locations[Random.Range(0, locations.Length)] };
        }
        return new[] { localization1, localization2 };
    }

    // Example method to retrieve the credits list
    public List<Contributors> GetCreditsList()
    {
        return creditsList;
    }

    // Example method to add a new contributor to a specific role
    public void AddContributor(string role, string contributorName)
    {
        Contributors roleContributors = creditsList.Find(c => c.contributorRole == role);

        if (roleContributors != null)
        {
            roleContributors.contributors.Add(contributorName);
        }
        else
        {
            // If the role doesn't exist, create a new one
            roleContributors = new Contributors
            {
                contributorRole = role,
                contributors = new List<string> { contributorName }
            };

            creditsList.Add(roleContributors);
        }
    }

    // Example method to remove a contributor from a specific role
    public void RemoveContributor(string role, string contributorName)
    {
        Contributors roleContributors = creditsList.Find(c => c.contributorRole == role);

        if (roleContributors != null)
        {
            roleContributors.contributors.Remove(contributorName);
        }
        // Handle if the role or contributor doesn't exist
    }
}
