﻿// For more information see https://aka.ms/fsharp-console-apps

// http://www.fssnip.net/4F/title/RedBlackTrees-with-insert
type Color = Red | Black

type 'a Tree =
    | Empty
    | Node of 'a TreeNode
and 'a TreeNode = { value: 'a; color: Color; left : 'a Tree; right : 'a Tree }

module RBTree =
    /// an empty RB-Tree
    let empty : 'a Tree = Empty

    /// member predicate
    /// please note: the compiler got issues if you use '==' on v and v'
    let rec isMember (t : 'a Tree) (v : 'a) : bool =
        match t with
        | Empty -> false
        | Node { value = v'; color = _; left = l; right = r } ->
            if v < v' then isMember l v
            else if v > v' then isMember r v
            else true

    /// inserts a new Element
    let insert (x : 'a) (t : 'a Tree) : 'a Tree =
        // force resulting node's color to be black
        let makeBlack = function
            | Node { value = y; color = _; left = a; right = b } -> Node { value = y; color = Black; left = a; right = b }
            | Empty -> failwith "unexpected case"

        let rec balance (color : Color) (a : 'a Tree) (x : 'a) (b : 'a Tree) =
            match (color, a, x, b) with
            | (Black, Node { value = y; color = Red; left = Node { value = x; color = Red; left = a; right = b }; right = c}, z, d)
            | (Black, Node { value = x; color = Red; left = a; right = Node { value = y; color = Red; left = b; right = c }; }, z, d)
            | (Black, a, x, Node { value = z; color = Red; left = Node { value = y; color = Red; left = b; right = c }; right = d; })
            | (Black, a, x, Node { value = y; color = Red; left = b; right = Node { value = z; color = Red; left = c; right = d }; })
                -> Node {value = y; color = Red; left = Node {value = x; color = Black; left = a; right = b}; right = Node {value = z; color = Black; left = c; right = d}}
            | _ -> Node { value = x; color = color; left = a; right = b }
        // recursive insert
        let rec ins t =
            match t with
            // initialise a new node's color to red
            | Empty -> Node { value = x; color = Red; left = Empty; right = Empty }
            | Node { value = y; color = color; left = a; right = b } ->
                if x < y then balance color (ins a) y b
                else if x > y then balance color a y (ins b)
                else Node { value = y; color = color; left = a; right = b }

        makeBlack (ins t)

    /// insert many values
    let insertMany (xs : 'a seq) (t : 'a Tree) : 'a Tree =
        let switch f = fun y x -> f x y
        xs |> Seq.fold (switch insert) t

printfn "Red Black Tree in F#"
let t = Empty |> RBTree.insertMany [2;5;8;7;10;3;4;1;9;6]
printfn t


