using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IpMonitor : MonoBehaviour {

	/// <summary>
	/// just a script to get an ip address for an interface element, can be probably done elsewhere
	/// </summary>
	void Start () {
        //get IPv4 address for client joining purposes
        this.transform.GetComponent<Text>().text = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork).ToString();
	}
}
