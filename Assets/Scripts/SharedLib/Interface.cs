public interface Action {

    void Activate(float value);

    void Deactivate();

    void Update_Action();

    void Set_Object();

    void Reset();

    void call_back_values(float heat, float max_heat, float power, float max_power, float fuel, float max_fuel);
}


public interface Reader {
     void UpdateDisplayData();
}