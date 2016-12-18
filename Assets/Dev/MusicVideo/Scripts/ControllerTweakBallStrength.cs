using UnityEngine;
using System.Collections;

public class ControllerTweakBallStrength : MonoBehaviour {

    public Rigidbody rigid;
    public SpringJoint spring;
    public ParticleSystem part;
    public SpriteRenderer spRend;
    public AudioSource aud;

    public float triggerRigidMass;
    public float triggerRigidDrag;
    public float triggerSpringSpring;
    public float triggerSpringDamp;
    public float particleAmount;
    public float particleSpeed;
    public Color triggerColor;

    float initialRigidMass;
    float initialRigidDrag;
    float initialSpringSpring;
    float initialSpringDamp;
    float initialParticleAmount;
    float initialParticleSpeed;
    Color initialColor;

    public float triggerTime = 3;
    float counter;
    bool pinged;

	// Use this for initialization
	void Start () {
        initialRigidMass = rigid.mass;
        initialRigidDrag = rigid.drag;
        initialSpringSpring = spring.spring;
        initialSpringDamp = spring.damper;
        initialParticleAmount = part.emissionRate;
        initialParticleSpeed = part.startSpeed;
        initialColor = spRend.color;
	}
	
    public void Ping()
    {
        if (!pinged)
        {
            rigid.mass = triggerRigidMass;
            rigid.drag = triggerRigidDrag;
            spring.spring = triggerSpringSpring;
            spring.damper = triggerSpringDamp;
            part.emissionRate = particleAmount;
            part.startSpeed = particleSpeed;
            spRend.color = triggerColor;
            aud.pitch = Random.Range(.6f, 1.2f);
            aud.Play();
            pinged = true;
            StartCoroutine(unSpring());
        }
    }

    IEnumerator unSpring()
    {
        while (counter < triggerTime)
        {
            counter += Time.deltaTime;
            float div = Mathf.SmoothStep(0,1, counter / triggerTime);
            rigid.mass = Mathf.Lerp(triggerRigidMass, initialRigidMass, div);
            rigid.drag = Mathf.Lerp(triggerRigidDrag, initialRigidDrag, div);
            spring.spring = Mathf.Lerp(triggerSpringSpring, initialSpringSpring, div);
            spring.damper = Mathf.Lerp(triggerSpringDamp, initialSpringDamp, div);
            part.emissionRate = Mathf.Lerp(particleAmount, initialParticleAmount, div);
            part.startSpeed = Mathf.Lerp(particleSpeed, initialParticleSpeed, div);
            spRend.color = Color.Lerp(triggerColor, initialColor, div);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        pinged = false;
        counter = 0;
    }

}
