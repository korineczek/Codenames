using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IpMonitor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //get IPv4 address for client joining purposes
        this.transform.GetComponent<Text>().text = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork).ToString();
	}
}
