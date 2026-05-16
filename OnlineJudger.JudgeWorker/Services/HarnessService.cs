namespace OnlineJudger.JudgeWorker.Services
{
    public class HarnessService
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
        public static string GetCSharpHarness()
        {
            return @"
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Reflection;
using System.Linq;

// === Структуры данных ===

public class ListNode
{
    public int val;
    public ListNode next;
    public ListNode(int val = 0, ListNode next = null)
    {
        this.val = val;
        this.next = next;
    }
}

public class TreeNode
{
    public int val;
    public TreeNode left;
    public TreeNode right;
    public TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
    {
        this.val = val;
        this.left = left;
        this.right = right;
    }
}

// === Десериализация ===

public static class Deserializer
{
    public static TreeNode DeserializeTree(JsonArray arr)
    {
        if (arr == null || arr.Count == 0) return null;
        if (arr[0] == null) return null;

        var root = new TreeNode(arr[0].GetValue<int>());
        var queue = new Queue<TreeNode>();
        queue.Enqueue(root);
        int i = 1;

        while (queue.Count > 0 && i < arr.Count)
        {
            var node = queue.Dequeue();
            if (i < arr.Count && arr[i] != null)
            {
                node.left = new TreeNode(arr[i].GetValue<int>());
                queue.Enqueue(node.left);
            }
            i++;
            if (i < arr.Count && arr[i] != null)
            {
                node.right = new TreeNode(arr[i].GetValue<int>());
                queue.Enqueue(node.right);
            }
            i++;
        }
        return root;
    }

    public static ListNode DeserializeList(JsonArray arr)
    {
        var dummy = new ListNode(0);
        var cur = dummy;
        foreach (var v in arr)
        {
            cur.next = new ListNode(v.GetValue<int>());
            cur = cur.next;
        }
        return dummy.next;
    }

    public static object Deserialize(JsonObject arg)
    {
        string type = arg[""type""].GetValue<string>();
        var value   = arg[""value""];

        return type switch
        {
            ""int""          => value.GetValue<int>(),
            ""float""        => value.GetValue<double>(),
            ""bool""         => value.GetValue<bool>(),
            ""string""       => value.GetValue<string>(),
            ""array""        => value.AsArray().Select(x => x.GetValue<int>()).ToArray(),
            ""matrix""       => value.AsArray()
                                   .Select(row => row.AsArray()
                                   .Select(x => x.GetValue<int>()).ToArray())
                                   .ToArray(),
            ""tree""         => DeserializeTree(value.AsArray()),
            ""linked_list""  => DeserializeList(value.AsArray()),
            _              => value
        };
    }
}

// === Сериализация ===

public static class Serializer
{
    public static JsonNode SerializeTree(TreeNode root)
    {
        var arr = new JsonArray();
        if (root == null) return arr;

        var queue = new Queue<TreeNode>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            if (node == null) { arr.Add(JsonValue.Create((int?)null)); continue; }
            arr.Add(node.val);
            queue.Enqueue(node.left);
            queue.Enqueue(node.right);
        }

        // убрать trailing null
        while (arr.Count > 0 && arr[arr.Count - 1] is JsonValue v && v.TryGetValue<int?>(out var val) && val == null)
            arr.RemoveAt(arr.Count - 1);

        return arr;
    }

    public static JsonNode SerializeList(ListNode head)
    {
        var arr = new JsonArray();
        while (head != null) { arr.Add(head.val); head = head.next; }
        return arr;
    }

    public static JsonNode Serialize(object result, string expectedType)
    {
        return expectedType switch
        {
            ""tree""        => SerializeTree(result as TreeNode),
            ""linked_list"" => SerializeList(result as ListNode),
            _             => JsonValue.Create(result as dynamic)
        };
    }
}

// === Харнесс ===

public static class Harness
{
    public static void Run(JsonObject testData)
    {
        string harnessType = testData[""harness_type""].GetValue<string>();
        var tc             = testData[""test_case""].AsObject();

        JsonNode result = null;

        if (harnessType == ""standard"")
        {
            string functionName = testData[""function_name""].GetValue<string>();
            string expectedType = tc[""expected""][""type""].GetValue<string>();
            var inputs          = tc[""inputs""].AsArray();

            var deserializedInputs = inputs
                .Select(x => Deserializer.Deserialize(x.AsObject()))
                .ToArray();

            var sol    = new Solution();
            var method = sol.GetType()
                            .GetMethods()
                            .First(m => m.Name.Equals(functionName, StringComparison.OrdinalIgnoreCase));

            var raw = method.Invoke(sol, deserializedInputs);
            result  = Serializer.Serialize(raw, expectedType);
        }
        else if (harnessType == ""design"")
        {
            string className = testData[""class_name""].GetValue<string>();
            var operations   = tc[""operations""].AsArray();
            var arguments    = tc[""arguments""].AsArray();

            var arr      = new JsonArray();
            object obj   = null;
            Type   type  = null;

            for (int i = 0; i < operations.Count; i++)
            {
                string op    = operations[i].GetValue<string>();
                var    args  = arguments[i].AsArray();

                if (obj == null)
                {
                    type = Assembly.GetExecutingAssembly()
                                   .GetTypes()
                                   .First(t => t.Name == className);

                    var ctorArgs = args.Select(x => (object)x.GetValue<int>()).ToArray();
                    obj          = Activator.CreateInstance(type, ctorArgs);
                    arr.Add(JsonValue.Create((int?)null));
                }
                else
                {
                    var method   = type.GetMethods()
                                       .First(m => m.Name.Equals(op, StringComparison.OrdinalIgnoreCase));
                    var argTypes = method.GetParameters();
                    var methodArgs = args.Select((x, idx) =>
                        Convert.ChangeType(x.GetValue<int>(), argTypes[idx].ParameterType)
                    ).ToArray();

                    var ret = method.Invoke(obj, methodArgs);
                    arr.Add(ret == null ? JsonValue.Create((int?)null) : JsonValue.Create(ret as dynamic));
                }
            }
            result = arr;
        }

        var output = new JsonObject { [""result""] = result };
        Console.WriteLine(output.ToJsonString());
    }
}

// === Точка входа ===

public static class Program
{
    public static void Main()
    {
        string stdin   = Console.In.ReadToEnd();
        var testData   = JsonNode.Parse(stdin).AsObject();
        Harness.Run(testData);
    }
}

// === Код пользователя вставляется сюда ===
";
        }
        public static string GetJavaHarness()
        {
            return @"
import org.json.*;
import java.util.*;
import java.io.*;
import java.lang.reflect.*;
import java.nio.charset.StandardCharsets;

// === Структуры данных ===

class ListNode {
    int val;
    ListNode next;
    ListNode(int val) { this.val = val; }
}

class TreeNode {
    int val;
    TreeNode left, right;
    TreeNode(int val) { this.val = val; }
}

// === Десериализация ===

class Deserializer {

    static TreeNode deserializeTree(JSONArray arr) {
        if (arr == null || arr.length() == 0) return null;
        if (arr.isNull(0)) return null;

        TreeNode root = new TreeNode(arr.getInt(0));
        Queue<TreeNode> queue = new LinkedList<>();
        queue.add(root);
        int i = 1;

        while (!queue.isEmpty() && i < arr.length()) {
            TreeNode node = queue.poll();
            if (i < arr.length() && !arr.isNull(i)) {
                node.left = new TreeNode(arr.getInt(i));
                queue.add(node.left);
            }
            i++;
            if (i < arr.length() && !arr.isNull(i)) {
                node.right = new TreeNode(arr.getInt(i));
                queue.add(node.right);
            }
            i++;
        }
        return root;
    }

    static ListNode deserializeList(JSONArray arr) {
        ListNode dummy = new ListNode(0);
        ListNode cur = dummy;
        for (int i = 0; i < arr.length(); i++) {
            cur.next = new ListNode(arr.getInt(i));
            cur = cur.next;
        }
        return dummy.next;
    }

    static Object deserialize(JSONObject arg) {
        String type   = arg.getString(""type"");
        Object value  = arg.get(""value"");

        switch (type) {
            case ""int"":         return arg.getInt(""value"");
            case ""float"":       return arg.getDouble(""value"");
            case ""bool"":        return arg.getBoolean(""value"");
            case ""string"":      return arg.getString(""value"");
            case ""array"": {
                JSONArray arr = arg.getJSONArray(""value"");
                int[] result  = new int[arr.length()];
                for (int i = 0; i < arr.length(); i++) result[i] = arr.getInt(i);
                return result;
            }
            case ""matrix"": {
                JSONArray rows  = arg.getJSONArray(""value"");
                int[][] result  = new int[rows.length()][];
                for (int i = 0; i < rows.length(); i++) {
                    JSONArray row = rows.getJSONArray(i);
                    result[i]     = new int[row.length()];
                    for (int j = 0; j < row.length(); j++) result[i][j] = row.getInt(j);
                }
                return result;
            }
            case ""string_matrix"": {
                JSONArray rows    = arg.getJSONArray(""value"");
                char[][] result   = new char[rows.length()][];
                for (int i = 0; i < rows.length(); i++) {
                    JSONArray row = rows.getJSONArray(i);
                    result[i]     = new char[row.length()];
                    for (int j = 0; j < row.length(); j++) result[i][j] = row.getString(j).charAt(0);
                }
                return result;
            }
            case ""tree"":        return deserializeTree(arg.getJSONArray(""value""));
            case ""linked_list"": return deserializeList(arg.getJSONArray(""value""));
            default:            return value;
        }
    }
}

// === Сериализация ===

class Serializer {

    static JSONArray serializeTree(TreeNode root) {
        JSONArray arr = new JSONArray();
        if (root == null) return arr;

        Queue<TreeNode> queue = new LinkedList<>();
        queue.add(root);

        while (!queue.isEmpty()) {
            TreeNode node = queue.poll();
            if (node == null) { arr.put(JSONObject.NULL); continue; }
            arr.put(node.val);
            queue.add(node.left);
            queue.add(node.right);
        }

        // убрать trailing null
        while (arr.length() > 0 && arr.isNull(arr.length() - 1))
            arr.remove(arr.length() - 1);

        return arr;
    }

    static JSONArray serializeList(ListNode head) {
        JSONArray arr = new JSONArray();
        while (head != null) { arr.put(head.val); head = head.next; }
        return arr;
    }

    static Object serialize(Object result, String expectedType) {
        switch (expectedType) {
            case ""tree"":        return serializeTree((TreeNode) result);
            case ""linked_list"": return serializeList((ListNode) result);
            case ""array"": {
                JSONArray arr = new JSONArray();
                if (result instanceof int[]) {
                    for (int v : (int[]) result) arr.put(v);
                } else if (result instanceof List) {
                    for (Object v : (List<?>) result) arr.put(v);
                }
                return arr;
            }
            case ""matrix"": {
                JSONArray rows = new JSONArray();
                if (result instanceof int[][]) {
                    for (int[] row : (int[][]) result) {
                        JSONArray r = new JSONArray();
                        for (int v : row) r.put(v);
                        rows.put(r);
                    }
                }
                return rows;
            }
            default: return result;
        }
    }
}

// === Харнесс ===

class Harness {

    static void run(JSONObject testData) throws Exception {
        String harnessType = testData.getString(""harness_type"");
        JSONObject tc      = testData.getJSONObject(""test_case"");

        Object result = null;

        if (harnessType.equals(""standard"")) {
            String functionName = testData.getString(""function_name"");
            String expectedType = tc.getJSONObject(""expected"").getString(""type"");
            JSONArray inputs    = tc.getJSONArray(""inputs"");

            Object[] deserializedInputs = new Object[inputs.length()];
            for (int i = 0; i < inputs.length(); i++)
                deserializedInputs[i] = Deserializer.deserialize(inputs.getJSONObject(i));

            Solution sol   = new Solution();
            Method method  = null;
            for (Method m : sol.getClass().getMethods())
                if (m.getName().equals(functionName)) { method = m; break; }

            result = Serializer.serialize(method.invoke(sol, deserializedInputs), expectedType);

        } else if (harnessType.equals(""design"")) {
            String className   = testData.getString(""class_name"");
            JSONArray operations = tc.getJSONArray(""operations"");
            JSONArray arguments  = tc.getJSONArray(""arguments"");

            JSONArray results  = new JSONArray();
            Object obj         = null;
            Class<?> clazz     = null;

            for (int i = 0; i < operations.length(); i++) {
                String op      = operations.getString(i);
                JSONArray args = arguments.getJSONArray(i);

                if (obj == null) {
                    clazz          = Class.forName(className);
                    Object[] cArgs = new Object[args.length()];
                    for (int j = 0; j < args.length(); j++) cArgs[j] = args.getInt(j);
                    obj = clazz.getConstructors()[0].newInstance(cArgs);
                    results.put(JSONObject.NULL);
                } else {
                    Method m = null;
                    for (Method candidate : clazz.getMethods())
                        if (candidate.getName().equals(op)) { m = candidate; break; }

                    Object[] mArgs = new Object[args.length()];
                    Parameter[] params = m.getParameters();
                    for (int j = 0; j < args.length(); j++)
                        mArgs[j] = convertArg(args, j, params[j].getType());

                    Object ret = m.invoke(obj, mArgs);
                    results.put(ret == null ? JSONObject.NULL : ret);
                }
            }
            result = results;
        }

        JSONObject output = new JSONObject();
        output.put(""result"", result);
        System.out.println(output.toString());
    }

    static Object convertArg(JSONArray args, int idx, Class<?> targetType) {
        if (targetType == int.class || targetType == Integer.class) return args.getInt(idx);
        if (targetType == double.class || targetType == Double.class) return args.getDouble(idx);
        if (targetType == boolean.class || targetType == Boolean.class) return args.getBoolean(idx);
        if (targetType == String.class) return args.getString(idx);
        return args.get(idx);
    }

    public static void main(String[] args) throws Exception {
        BufferedReader reader = new BufferedReader(new InputStreamReader(System.in, StandardCharsets.UTF_8));
        StringBuilder sb      = new StringBuilder();
        String line;
        while ((line = reader.readLine()) != null) sb.append(line);

        JSONObject testData = new JSONObject(sb.toString());
        run(testData);
    }
}

// === Код пользователя вставляется сюда ===
";
        }
    }
}
