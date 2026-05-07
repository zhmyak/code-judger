using OnlineJudger.Domain.Entities;

namespace OnlineJudger.JudgeWorker.Services
{
    public class Harness
    {
        public static string GetPythonHarness()
        {
            return @"
import json
import sys
from collections import deque

# Структуры данных

class ListNode:
    def __init__(self, val=0, next=None):
        self.val = val
        self.next = next

class TreeNode:
    def __init__(self, val=0, left=None, right=None):
        self.val = val
        self.left = left
        self.right = right

# Десериализация 

def deserialize_tree(data):
    if not data:
        return None
    root = TreeNode(data[0])
    queue = deque([root])
    i = 1
    while queue and i < len(data):
        node = queue.popleft()
        if i < len(data) and data[i] is not None:
            node.left = TreeNode(data[i])
            queue.append(node.left)
        i += 1
        if i < len(data) and data[i] is not None:
            node.right = TreeNode(data[i])
            queue.append(node.right)
        i += 1
    return root

def deserialize_list(data):
    dummy = ListNode(0)
    cur = dummy
    for val in data:
        cur.next = ListNode(val)
        cur = cur.next
    return dummy.next

def deserialize(arg):
    t = arg[""type""]
    v = arg[""value""]
    if t == ""tree"":        return deserialize_tree(v)
    if t == ""linked_list"": return deserialize_list(v)
    return v

# Сериализация

def serialize_tree(root):
    if not root:
        return []
    result, queue = [], deque([root])
    while queue:
        node = queue.popleft()
        if node:
            result.append(node.val)
            queue.append(node.left)
            queue.append(node.right)
        else:
            result.append(None)
    while result and result[-1] is None:
        result.pop()
    return result

def serialize_list(head):
    result = []
    while head:
        result.append(head.val)
        head = head.next
    return result

def serialize(result, expected_type):
    if expected_type == ""tree"":        return serialize_tree(result)
    if expected_type == ""linked_list"": return serialize_list(result)
    return result

# Запуск

def run(test_data):
    harness_type = test_data[""harness_type""]
    tc           = test_data[""test_case""]

    if harness_type == ""standard"":
        inputs     = [deserialize(arg) for arg in tc[""inputs""]]
        raw_result = getattr(Solution(), test_data[""function_name""])(*inputs)
        result     = serialize(raw_result, tc[""expected""][""type""])

    elif harness_type == ""design"":
        ops    = tc[""operations""]
        args   = tc[""arguments""]
        obj    = None
        result = []
        for op, arg in zip(ops, args):
            if obj is None:
                obj = globals()[test_data[""class_name""]](*arg)
                result.append(None)
            else:
                result.append(getattr(obj, op)(*arg))

    print(json.dumps({""result"": result}))

if __name__ == ""__main__"":
    test_data = json.loads(sys.stdin.read())
    run(test_data)";
        }
    }
}
