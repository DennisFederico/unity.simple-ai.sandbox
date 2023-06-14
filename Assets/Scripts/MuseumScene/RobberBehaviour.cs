using System.Collections.Generic;
using System.Numerics;
using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

namespace MuseumScene {
    public class RobberBehaviour : MonoBehaviour {
        private readonly BehaviourTree.BehaviourTree _tree = new("Robber");
        private NavMeshAgent _navMeshAgent;
        [Range(0f, 1000f)] [SerializeField] private float money = 800;
        [SerializeField] private List<GameObject> valuableObjects;
        private GameObject _targetValuable;
        [SerializeField] private GameObject van;
        [SerializeField] private GameObject frontDoor;
        [SerializeField] private GameObject backDoor;
        private BTNode.NodeState _treeState;

        private void Awake() {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            Time.timeScale = 2f;
        }

        private void Start() {
            var openFrontDoor = new BTSequenceNode("TryFrontDoor")
                .AddChild(new BTLeafNode("GoToFrontDoor", () => MoveAction(frontDoor.transform.position)))
                .AddChild(new BTLeafNode("OpenFrontDoor", () => OpenDoorAction(frontDoor)));

            var openBackDoor = new BTSequenceNode("TryBackDoor")
                .AddChild(new BTLeafNode("GoToBackDoor", () => MoveAction(backDoor.transform.position)))
                .AddChild(new BTLeafNode("OpenBackDoor", () => OpenDoorAction(backDoor)));

            var enterMuseumSelector = new BTSelectorNode("EnterMuseum")
                .AddChild(openFrontDoor)
                .AddChild(openBackDoor);

            var chooseValuable = new BTLeafNode("ChooseValuable", () => {
                if (valuableObjects?.Count == 0) return BTNode.NodeState.FAILURE;
                var index = Random.Range(0, valuableObjects.Count - 1);
                Debug.Log($"Steal Index {index}");
                _targetValuable = valuableObjects[index];
                valuableObjects.RemoveAt(index);
                return BTNode.NodeState.SUCCESS;
            });

            var goToValuable = new BTLeafNode("GoToValuable", () => MoveAction(_targetValuable.transform.position));
            
            var grabValuable = new BTLeafNode("GrabValuable", () => {
                _targetValuable.transform.parent = this.transform;
                return BTNode.NodeState.SUCCESS;
            });
            
            var goToVan = new BTLeafNode("GoToVan", () => MoveAction(van.transform.position));

            _tree.AddChild(
                new BTSequenceNode("StealDiamondSequence")
                    .AddChild(new BTLeafNode("NeedsMoneyCondition", () => {
                        if (money <= 800) return BTNode.NodeState.SUCCESS;
                        return BTNode.NodeState.FAILURE;
                    }))
                    .AddChild(enterMuseumSelector)
                    .AddChild(chooseValuable)
                    .AddChild(goToValuable)
                    .AddChild(grabValuable)
                    .AddChild(goToVan)
                    .AddChild(new BTLeafNode("CashLoot", () => {
                        Destroy(_targetValuable);
                        money += 200;
                        return BTNode.NodeState.SUCCESS;
                    }))
                    .AddChild(new BTLeafNode("HasEnoughMoney", () => {
                        if (money >= 800) return BTNode.NodeState.SUCCESS;
                        return BTNode.NodeState.FAILURE;
                    }))
            );


            // _tree.AddChild(new BTLeafNode("Goto", () => MoveAction(diamond.transform.position)));

            _tree.DebugTree();
        }

        private void Update() {
            
            if (_treeState is BTNode.NodeState.SUCCESS) return;
            _treeState = _tree.Evaluate();
        }

        private BTNode.NodeState OpenDoorAction(GameObject door, float pickLockLevel = 10f) {
            if (door.TryGetComponent<DoorLock>(out var doorLock) && doorLock.isLocked) {
                if (!doorLock.TryPickLock(pickLockLevel)) return BTNode.NodeState.FAILURE;
            }

            door.SetActive(false);
            return BTNode.NodeState.SUCCESS;
        }

        private BTNode.NodeState MoveAction(Vector3 position) {
            if (_navMeshAgent.isStopped) {
                _navMeshAgent.SetDestination(position);
                _navMeshAgent.isStopped = false;
                return BTNode.NodeState.RUNNING;
            }

            if (_navMeshAgent.pathPending) return BTNode.NodeState.RUNNING;

            if (_navMeshAgent.pathStatus is NavMeshPathStatus.PathInvalid) {
                _navMeshAgent.isStopped = true;
                return BTNode.NodeState.FAILURE;
            }

            if (_navMeshAgent.hasPath && _navMeshAgent.pathStatus is NavMeshPathStatus.PathComplete && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance) {
                if (_navMeshAgent.pathStatus is NavMeshPathStatus.PathComplete) {
                    // Debug.Log("Reached destination");
                    _navMeshAgent.isStopped = true;
                    return BTNode.NodeState.SUCCESS;
                }
            }

            if (_navMeshAgent.pathStatus is NavMeshPathStatus.PathPartial) {
                if (Vector3.Distance(transform.position, position) <= _navMeshAgent.stoppingDistance * 2) {
                    // Debug.Log("Reached destination");
                    _navMeshAgent.isStopped = true;
                    return BTNode.NodeState.SUCCESS;
                }

                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance) {
                    // Debug.Log("Cannot reach the destination");
                    _navMeshAgent.isStopped = true;
                    return BTNode.NodeState.FAILURE;
                }
            }

            _navMeshAgent.SetDestination(position);
            return BTNode.NodeState.RUNNING;
        }
    }
}