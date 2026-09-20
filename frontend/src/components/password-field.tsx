"use client";

import { Eye, EyeOff } from "lucide-react";
import { useState } from "react";

type PasswordFieldProps = {
  id: string;
  name: string;
  label: string;
  autoComplete: "current-password" | "new-password";
  minLength: number;
  maxLength?: number;
  describedBy?: string;
};

export function PasswordField({ id, name, label, autoComplete, minLength, maxLength = 128, describedBy }: PasswordFieldProps) {
  const [visible, setVisible] = useState(false);

  return <div className="password-field">
    <label htmlFor={id}>{label}</label>
    <div className="password-input-wrap">
      <input id={id} name={name} type={visible ? "text" : "password"} autoComplete={autoComplete} required minLength={minLength} maxLength={maxLength} aria-describedby={describedBy} />
      <button type="button" className="password-visibility" onClick={() => setVisible(value => !value)} aria-label={visible ? `Hide ${label.toLowerCase()}` : `Show ${label.toLowerCase()}`} aria-pressed={visible}>
        {visible ? <EyeOff aria-hidden="true" /> : <Eye aria-hidden="true" />}
      </button>
    </div>
  </div>;
}
