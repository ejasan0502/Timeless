// MIT License (MIT) - Copyright (c) 2014 jakevn - Please see included LICENSE file
using MassiveNet;
using UnityEngine;

namespace Massive.Examples.NetAdvanced {

    public class PlayerOwner : MonoBehaviour {

        private NetView view;

        public string PlayerName { get; private set; }
        public int Hp { get; private set; }

        void Awake() {
            view = GetComponent<NetView>();

            view.OnReadInstantiateData += Instantiate;
        }

        void Instantiate(NetStream stream) {
            PlayerName = stream.ReadString();
            Hp = stream.ReadInt();
            Vector3 pos = stream.ReadVector3();
            // Prevemt jumpiness during handoff by ignoring position data if similar enough:
            if (transform.position != Vector3.zero && Vector3.Distance(transform.position, pos) < 5) return;
            transform.position = pos;
        }

    }

}
