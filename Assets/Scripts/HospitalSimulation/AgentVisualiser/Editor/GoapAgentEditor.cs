using System.Collections.Generic;
using HospitalSimulation.Goap;
using UnityEditor;
using UnityEngine;

namespace HospitalSimulation.AgentVisualiser.Editor {
    [CustomEditor(typeof(GoapAgentVisual))]
    [CanEditMultipleObjects]
    public class GoapAgentVisualEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            if (target == null) return;
            DrawDefaultInspector();
            serializedObject.Update();
            GoapAgentVisual agent = (GoapAgentVisual)target;
            GUILayout.Label("Name: " + agent.name);
            GUILayout.Label("Current Action: " + agent.gameObject.GetComponent<GoapAgent>().currentAction);
            GUILayout.Label("Actions: ");
            var actionsQueue = agent.gameObject.GetComponent<GoapAgent>().ActionsQueue;
            if (actionsQueue is { Count: > 0 }) {
                GUILayout.Label("Queue: ");
                foreach (GoapAction a in actionsQueue) {
                    string pre = "";
                    string eff = "";
                    foreach (KeyValuePair<string, int> p in a.Preconditions)
                        pre += p.Key + ", ";
                    foreach (KeyValuePair<string, int> e in a.AfterEffects)
                        eff += e.Key + ", ";
                    GUILayout.Label("====  " + a.Name + "(" + pre + ")(" + eff + ")");
                }
            }
            GUILayout.Label("Goals: ");
            foreach (KeyValuePair<SubGoal, int> g in agent.gameObject.GetComponent<GoapAgent>().goals) {
                GUILayout.Label("---: ");
                foreach (KeyValuePair<string, int> sg in g.Key.sGoals)
                    GUILayout.Label("=====  " + sg.Key);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}