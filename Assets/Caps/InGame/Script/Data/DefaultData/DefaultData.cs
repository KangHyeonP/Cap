
public enum ECharacters
{
    Haeseong,
    Eunha,
    Kuiper, 
    c2, 
    c3
}

// �⺻���� ������ ���� �����ϴ� Ŭ���� ex)���� �÷��̾��� �ɷ�ġ

public class DefaultData
{
    // Characters
    private CharacterData characters;
    public CharacterData Characters => characters;

    // CurIndex
    private ECharacters enumC;

    public DefaultData()
    {
        characters = new CharacterData();
    }

    public void SettingValue(int index)
    {
        characters.UpdateData(index);
    }
}

// �⺻ ĳ���� Ư��
public class CharacterData
{
    // ����, ����, 1, 2, 3 -> Characters, �̸� �� �������
    private int[] defaultHp = { 6, 4, 8, 1, 1 };
    private int[] defaultAttackPower = { 10, 8, 12, 4, 4 };
    private float[] defaultAimAccuracy = { 5, 7, 3, 3, 3 };
    private float[] defaultBulletDistance = { 0.2f, 0.3f, 0.1f, 0.1f, 0.1f };
    private float[] defaultSpeed = { 3.5f, 4.0f, 2.5f, 2, 2 };
    private float[] defaultAttackDealy = { 10.0f, 12.0f, 8, 6, 6 };

    public void UpdateData(int i)
    {
        if (DataManager.Instacne.JsonClass._PlayerData.playerLock[i])
        GameManager.Instance.GetPlayerValue(defaultHp[i], defaultAttackPower[i],
            defaultAimAccuracy[i], defaultBulletDistance[i], defaultSpeed[i],
            defaultAttackDealy[i]);
    }
}
