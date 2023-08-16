using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    //bat dau vao state
    void OnEnter(Enemy enemy);

    //update state
    void OnExecute(Enemy enemy);

    //thoat khoi state
    void OnExit(Enemy enemy);


}
